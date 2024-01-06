using System;
using System.Windows;

namespace diploma_thesis
{
    public partial class ResultsWindow : Window
    {
        private readonly int correctAnswers;
        private readonly int totalQuestions;

        public ResultsWindow(int correctAnswers, int totalQuestions)
        {
            InitializeComponent();
            this.correctAnswers = correctAnswers;
            this.totalQuestions = totalQuestions;
            UpdateResults();
        }

        private void UpdateResults()
        {
            CorrectAnswersLabel.Content = $"Правильных ответов: {correctAnswers}";
            TotalQuestionsLabel.Content = $"Всего вопросов: {totalQuestions}";
            double percentage = (double)correctAnswers / totalQuestions * 100;
            PercentageLabel.Content = $"Процент правильных ответов: {percentage:F2}%";
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new ();
            mainWindow.Show();
            Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
