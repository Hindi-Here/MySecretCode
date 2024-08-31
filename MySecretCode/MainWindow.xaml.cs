using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MySecretCode
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string[] codeReader = File.ReadAllLines(@"code.txt"); //для определения существующего алфавита
            for (int i = 0; i < codeReader.Length; i++)
                wordBook.Text += codeReader[i] + "\n";
        }

        private void AddSymbol_Click(object sender, RoutedEventArgs e) //добавление символов в алфавит
        {
            bool SearchSymbol = false; // булевая для объявления флага "есть повторения / нет повторений" 
            string[] codeReader = File.ReadAllLines(@"code.txt"); //для динамического определения длины алфавита

            //отслеживание повторяются ли значения алфавита или нет
            for (int i = 0; i < codeReader.Length; i++)
            {
                if (SymbolInput.Text == codeReader[i].Split(' ')[0] || SymbolOutput.Text == codeReader[i].Split(' ')[2])
                {
                    SearchSymbol = true;
                    break;
                }
            }
            // --- //

            if (SymbolInput.Text != "" && SymbolOutput.Text != "" && SearchSymbol == false)
            {
                WarningText.Visibility = Visibility.Hidden;
                // Запись в текстовый документ алфавита //
                using (StreamWriter codeWriter = new StreamWriter(@"code.txt"))
                {
                    for (int i = 0; i < codeReader.Length; i++)
                        codeWriter.WriteLine(codeReader[i]);

                    codeWriter.Write(SymbolInput.Text + " = " + SymbolOutput.Text);
                    wordBook.Text += SymbolInput.Text + " = " + SymbolOutput.Text + "\n";
                }
                // --- //
            }
            else
                WarningText.Visibility = Visibility.Visible;

            SymbolsClear();
        }

        private void Change_WordBook_Click(object sender, RoutedEventArgs e) //сохранение изменений в алфавите
        {
            string[] ChangeWordBook = wordBook.Text.Split('\n');
            using (StreamWriter codeWriter = new StreamWriter(@"code.txt"))
            {
                for (int i = 0; i < ChangeWordBook.Length; i++)
                    codeWriter.WriteLine(ChangeWordBook[i]);

            }

        }

        private void ClearSymbols_Click(object sender, RoutedEventArgs e) //очистка граф символов
        {
            WarningText.Visibility = Visibility.Hidden;
            SymbolsClear();
        }

        private void Clear_WordBook_Click(object sender, RoutedEventArgs e) //очистка значений алфавита
        {
            wordBook.Text = "";

            var codeWriter = new StreamWriter(@"code.txt");
            codeWriter.Write("");
        }


        // динамическая расшифровка и дешифровка //


        private void CodeBox_Copy_TextChange(object sender, TextChangedEventArgs e)
        {
            unCode_Box_Copy.Text = "";
            char[] wordsInBox = codeBox_Copy.Text.ToCharArray();
            int[] numbSPL = { 0, 2 };
            TextBox MethodBox = unCode_Box_Copy;
            SearchSymbol(wordsInBox, numbSPL, MethodBox);
        }

        private void UnBox_TextChange(object sender, TextChangedEventArgs e)
        {
            codeBox.Text = "";
            char[] wordsInBox = unCode_Box.Text.ToCharArray();
            int[] numbSPL = {2,0};
            TextBox MethodBox = codeBox;
            SearchSymbol(wordsInBox, numbSPL, MethodBox);
        }


        // переключение между режимами шифровки и дешифровки //


        private void CodeBox_Checked(object sender, RoutedEventArgs e)
        {
            codeBox_Copy.Text = "";
            unCode_Box_Copy.Text = "";

            codeBox_Copy.Visibility = Visibility.Visible;
            unCode_Box_Copy.Visibility = Visibility.Visible;

            checkCodeBox.Content = "ON";
            checkUnCodeBox.Content = "OFF"; 
            unCode_Box.IsReadOnly = true;
        }

        private void UnCodeBox_Checked(object sender, RoutedEventArgs e)
        {
            codeBox.Text = "";
            unCode_Box.Text = "";

            codeBox_Copy.Visibility = Visibility.Hidden;
            unCode_Box_Copy.Visibility = Visibility.Hidden;

            checkCodeBox.Content = "OFF";
            checkUnCodeBox.Content = "ON"; 
            unCode_Box.IsReadOnly = false;
        }


        void SearchSymbol(char[] wordsInBox, int[] numbSPL ,TextBox MethodBox) //метод шифровки и дешифровки
        {
            string[]codeReader = File.ReadAllLines(@"code.txt");
            for (int i = 0; i < wordsInBox.Length; i++)
            {
                bool SearchSymbol = false;
                for (int j = 0; j < codeReader.Length; j++)
                {
                    if (wordsInBox[i].ToString() == codeReader[j].Split(' ')[numbSPL[0]])
                    {
                        MethodBox.Text += codeReader[j].Split(' ')[numbSPL[1]];
                        SearchSymbol = true;
                    }
                }
                if (SearchSymbol == false)
                    MethodBox.Text += "*";
            }
        } 

        void SymbolsClear() // метод очистки символов
        {
            SymbolInput.Clear();
            SymbolOutput.Clear();
        }
    }
}
