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
        List<string> historyMoves = new List<string>();

        // метод начала игры
        private void buttonStart(object sender, EventArgs e)
        {
            if(textBox1.Text == "" || textBox2.Text == "") // проверка на наличие данных в обоих полях ввода
            {
                MessageBox.Show("Вы не ввели в нужные поля количество фишек для начала игры", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            textBox5.Enabled = true;
            textBox6.Enabled = true;
            label14.Text = "";
            label15.Text = "";
            if (!playerMove) makesMoveAI();
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
                if (playerMove) MessageBox.Show("Вы победили", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else MessageBox.Show("Вы проиграли", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // метод для совершения хода игроком
        private void makeMove(object sender, EventArgs e) 
        {
            if (textBox5.Text == "" && textBox6.Text == "") {MessageBox.Show("Хотя бы одно поле должно быть заполнено", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; } // строка для проверки на хотя бы одно введенное значение
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
                    MessageBox.Show("Вы не можете изменять разное количество фишек в двух кучках за раз", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else { MessageBox.Show("Недостаточно фишек в кучках!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            historyMoves.Add($"Игрок сделал ход: {x}, {y}");

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
            historyMoves.Clear();
        }

        private void makesMoveAI()
        {
        
        }

        // ограничения на ввод посторонних символов в textbox
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || (int)e.KeyChar == 8))
                e.KeyChar = (char)0;
        }

        private void exitMenu(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Действительно выйти?", "Выход из программы",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes) Close();
            Close();
        }

        private void rulesMenu(object sender, EventArgs e)
        {
            MessageBox.Show(
                "ПРАВИЛА ИГРЫ ВИТХОФФА\n\n" +
                "Игра ведётся между игроком и компьютером. На игровом поле расположены две кучки фишек.\n\n" +
                "Цель игры:\n" +
                "Сделать последний возможный ход и оставить на поле 0 фишек в обеих кучках.\n\n" +
                "Правила хода:\n" +
                "За один ход разрешается выполнить одно из следующих действий:\n" +
                "• убрать любое количество фишек из первой кучки;\n" +
                "• убрать любое количество фишек из второй кучки;\n" +
                "• убрать одинаковое количество фишек одновременно из обеих кучек.\n\n" +
                "Запрещается:\n" +
                "• убирать больше фишек, чем находится в кучке;\n" +
                "• убирать разное количество фишек из обеих кучек одновременно;\n" +
                "• пропускать ход.\n\n" +
                "Завершение игры:\n" +
                "Игра заканчивается, когда обе кучки становятся пустыми.\n" +
                "Победителем считается игрок, сделавший последний допустимый ход.\n\n" +
                "Совет:\n" +
                "Компьютер использует выигрышную стратегию, основанную на математической теории игры Витхоффа. " +
                "Для победы старайтесь оставлять сопернику невыгодные позиции.",
                "Правила игры Витхоффа",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void historyMovesMenu(object sender, EventArgs e)
        {
            Form hm = new Form2(historyMoves);
            hm.Show();
        }
    }
}
