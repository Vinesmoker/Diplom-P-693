using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace diploma_thesis
{
    public partial class MainWindow : Window
    {
        private List<QuestionData> questionsList = new List<QuestionData>();
        private Random random = new Random();
        private int answeredQuestions = 0;
        private const int TotalQuestions = 30;
        private int correctAnswers = 0;

        public MainWindow()
        {
            InitializeComponent();
            ReadQuestionsFromFile();
            SetRandomQuestion();
        }

        private void ReadQuestionsFromFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(@"C:\Users\harry\source\repos\diploma thesis\diploma thesis\Images1\Questions.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        string? question = sr.ReadLine();
                        string? answer1 = sr.ReadLine();
                        string? answer2 = sr.ReadLine();
                        string? answer3 = sr.ReadLine();
                        string? answer4 = sr.ReadLine();
                        string? correctAnswer = sr.ReadLine();

                        if (question != null && answer1 != null && answer2 != null && answer3 != null && answer4 != null && correctAnswer != null)
                        {
                            questionsList.Add(new QuestionData(question, answer1, answer2, answer3, answer4, correctAnswer));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения вопросов из файла: {ex.Message}");
            }
        }

        private void SetRandomQuestion()
        {
            if (answeredQuestions < TotalQuestions)
            {
                QuestionData randomQuestion = GetUnusedRandomQuestion();
                if (randomQuestion != null)
                {
                    Question.Text = randomQuestion.Question;
                    Answer1.Content = randomQuestion.Answer1;
                    Answer2.Content = randomQuestion.Answer2;
                    Answer3.Content = randomQuestion.Answer3;
                    Answer4.Content = randomQuestion.Answer4;
                    Answer1.Click -= Answer_Click;
                    Answer2.Click -= Answer_Click;
                    Answer3.Click -= Answer_Click;
                    Answer4.Click -= Answer_Click;
                    Answer1.Click += Answer_Click;
                    Answer2.Click += Answer_Click;
                    Answer3.Click += Answer_Click;
                    Answer4.Click += Answer_Click;
                    UpdateInfoPanel();
                }
                else
                {
                    MessageBox.Show("Все вопросы использованы.");
                }
            }
            else
            {
                ShowResultsWindow();
            }
        }

        private QuestionData GetUnusedRandomQuestion()
        {
            List<QuestionData> unusedQuestions = questionsList.FindAll(q => !q.Used);
            if (unusedQuestions.Count > 0)
            {
                int randomIndex = random.Next(unusedQuestions.Count);
                QuestionData randomQuestion = unusedQuestions[randomIndex];
                randomQuestion.Used = true;
                return randomQuestion;
            }
            return null;
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            string? selectedAnswer = clickedButton.Content?.ToString();
            QuestionData currentQuestion = GetCurrentQuestion();
            if (currentQuestion != null && selectedAnswer == currentQuestion.CorrectAnswer)
            {
                correctAnswers++;
            }
            SetRandomQuestion();
            answeredQuestions++;
            UpdateInfoPanel();
        }

        private void UpdateInfoPanel()
        {
            InfoPanel.Text = $"Вопросы: {answeredQuestions}/{TotalQuestions}";
        }

        private void ShowResultsWindow()
        {
            ResultsWindow resultsWindow = new ResultsWindow(correctAnswers, TotalQuestions);
            resultsWindow.ShowDialog();
            Close();
        }

        private QuestionData GetCurrentQuestion()
        {
            return questionsList.FirstOrDefault(q => q.Used && q.Question == Question.Text);
        }
    }

    public class QuestionData
    {
        public string Question { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string CorrectAnswer { get; set; }
        public bool Used { get; set; }

        public QuestionData(string question, string answer1, string answer2, string answer3, string answer4, string correctAnswer)
        {
            Question = question;
            Answer1 = answer1;
            Answer2 = answer2;
            Answer3 = answer3;
            Answer4 = answer4;
            CorrectAnswer = correctAnswer;
            Used = false;
        }
    }
}
