using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace АС_Игра_Витхоффа
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int n = 0; // первая кучка
        int m = 0; // вторая кучка
        bool playerMove; // переменная хранящая булевое значение "ходит игрок или нет"

        // метод начала игры
        private void buttonStart(object sender, EventArgs e)
        {
            if(textBox1.Text == "" && textBox2.Text == "") // проверка на наличие данных в обоих полях ввода
            {
                MessageBox.Show("Вы не ввели в нужные поля количество фишек для начала игры", "Предупреждение");
                return;
            }

            n = int.Parse(textBox1.Text); // заносим данные с экрана в переменные и обновляем значения на экране
            m = int.Parse(textBox2.Text);
            playerMove = radioButton1.Checked;
            updatingValues();

            groupBox1.Enabled = false; // блокируем элементы которые предназначены для начала игры
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button1.Enabled = false;
        }

        // метод для обновления значений поля на экране
        private void updatingValues()
        {
            label12.Text = n.ToString(); // обновляются значения кучек на экране
            label13.Text = m.ToString();

            textBox5.Clear(); // поля ввода игрока обнуляются
            textBox6.Clear(); 

            if (n == 0) textBox5.Enabled = false; // при нулевом количестве фишек в кучке они блокируются для ввода
            if (m == 0) textBox6.Enabled = false;

            if (playerMove) label3.Text = "Ваш ход"; // изменение надписи текущего хода
            else label3.Text = "ход компьютера";
        }

        // метод проверки на завершение игры и получения победителя
        private void getCurrentWinner()
        {
            if (n <= 0 && m <= 0)
                if (playerMove) MessageBox.Show("Вы победили", "Уведомление");
                else MessageBox.Show("Вы проиграли", "Уведомление");
        }

        // метод для совершения хода игроком
        private void makeMove(object sender, EventArgs e) 
        {
            if (textBox5.Text == "" && textBox6.Text == "") {MessageBox.Show("Хотя бы одно поле должно быть заполнено", "Предупреждение"); return; } // строка для проверки на хотя бы одно введенное значение
            int x = 0;
            int y = 0;
            if (textBox5.Text != "") x = int.Parse(textBox5.Text); // записываем значения с экрана во временные переменные
            if (textBox6.Text != "") y = int.Parse(textBox6.Text);

            if (n >= x && m >= y && (x == y || x == 0 || y == 0)) // проверка на наличие достаточного количества фишек в кучке и проверка на равенство двух кучек или нулевое значение одной из кучек
            {
                n -= x;
                m -= y;
            }
            else 
                if (x != y && x != 0 && y != 0) // проверка на разное количество фишек в кучках
                {
                    MessageBox.Show("Вы не можете изменять разное количество фишек в двух кучках за раз", "Предупреждение");
                    return;
                }
                else { MessageBox.Show("Недостаточно фишек в кучках!", "Предупреждение"); return; }

            getCurrentWinner();
            playerMove = !playerMove;
            updatingValues();
            makesMoveAI();
        }

        // метод для начала игры заного
        private void resetGame(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите начать игру заного?", "Предупреждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.No) return;
            n = 0;
            m = 0;
            label3.Text = "";

            groupBox1.Enabled = true; // блокируем элементы которые предназначены для начала игры
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            button1.Enabled = true;
            textBox1.Text = "";
            textBox2.Text = "";


            updatingValues();
            // необходимо доделать рестарт игры
        }

        // метод для совершения хода ИИ
private bool IsLosingPosition(int a, int b)
{
    if (a > b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    const double GoldenRatio = 1.618033988749895;

    int k = b - a;
    int ak = (int)Math.Floor(k * GoldenRatio);

    return ak == a;
}

private void makesMoveAI()
{
    label14.Text = "";
    label15.Text = "";

    int originalN = n;
    int originalM = m;

    // 1. Уменьшаем первую кучу
    for (int newN = 0; newN < n; newN++)
    {
        if (IsLosingPosition(newN, m))
        {
            label14.Text = (n - newN).ToString();
            label15.Text = "0";

            n = newN;

            getCurrentWinner();
            playerMove = !playerMove;
            updatingValues();
            return;
        }
    }

    // 2. Уменьшаем вторую кучу
    for (int newM = 0; newM < m; newM++)
    {
        if (IsLosingPosition(n, newM))
        {
            label14.Text = "0";
            label15.Text = (m - newM).ToString();

            m = newM;

            getCurrentWinner();
            playerMove = !playerMove;
            updatingValues();
            return;
        }
    }

    // 3. Уменьшаем обе кучи одинаково
    int maxRemove = Math.Min(n, m);

    for (int remove = 1; remove <= maxRemove; remove++)
    {
        if (IsLosingPosition(n - remove, m - remove))
        {
            label14.Text = remove.ToString();
            label15.Text = remove.ToString();

            n -= remove;
            m -= remove;

            getCurrentWinner();
            playerMove = !playerMove;
            updatingValues();
            return;
        }
    }

    // Если вдруг проигрышная позиция и выигрышного хода нет,
    // делаем минимальный допустимый ход

    if (n > 0)
    {
        n--;

        label14.Text = "1";
        label15.Text = "0";
    }
    else if (m > 0)
    {
        m--;

        label14.Text = "0";
        label15.Text = "1";
    }

    getCurrentWinner();
    playerMove = !playerMove;
    updatingValues();
}

        // ограничения на ввод посторонних символов в textbox
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || (int)e.KeyChar == 8))
                e.KeyChar = (char)0;
        }
    }
}
