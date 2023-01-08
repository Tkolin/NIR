using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NIR
{
   public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ToolTip otm = new ToolTip();
            otm.SetToolTip(button1, "Отменяет загрузку изображения");
            ToolTip chb = new ToolTip();
            chb.SetToolTip(filterSelector, "Выбор фильтра");
            filterSelector.SelectedIndex = 0;
        }
        //Переменные
        public string path = null;
        Bitmap image; //Bitmap для открываемого изображения
        public static string full_name_of_image = "\0";
        public static UInt32[,] pixel;
        public int sch = 1;
        private void FilterBtn_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)// если изображение в pictureBox1 Нет
            {
                MessageBox.Show("Нет загруженного изображения! Загрузите его.", "Ошибка");
                return;
            }
            Filters filters = new Filters(image);
            switch (filterSelector.SelectedIndex)
            {
                case 0:
                    pictureBox2.Image = filters.Sepia();
                    break;
                case 1:
                    pictureBox2.Image = filters.BlackAndWhite();
                    break;
                case 2:
                    pictureBox2.Image = filters.Blur();
                    break;
                case 3:
                    pictureBox2.Image = filters.ShadesOfGrey();
                    break;
                case 4:
                    pictureBox2.Image = filters.Sharpness();
                    break;
                case 5:
                    pictureBox2.Image = filters.Negative();
                    break;
                case 6:
                    pictureBox2.Image = filters.Sobel();
                    break;
                case 7:
                    pictureBox2.Image = filters.Roberts();
                    break;
                case 8:
                    pictureBox2.Image = filters.AdaptiveNoiseReduction();
                    break;
                case 9:
                    pictureBox2.Image = filters.WienerFrequency();
                    break;
            }
            if (Сохранять.Checked)
            {
                if (path != null)
                {
                    pictureBox2.Image.Save(path + System.Guid.NewGuid() + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    MessageBox.Show("Изображение готово!\nСохранение: Удачно.", "Уведомление");
                }
                else
                {
                    MessageBox.Show("Невозможно сохранить изображение\nОшибка: Не задан путь.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Изображение готово!", "Уведомление");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            MessageBox.Show("Изображение удаленно", "Уведомление");

        }
        private void Сохранять_CheckedChanged(object sender, EventArgs e)
        {
            if (Сохранять.Checked == true)
            {
                if(path != null)
                {
                    MessageBox.Show("Изображение будет сохранено, чтобы отменить, уберите чекбокс", "Уведомление");
                }
                else
                {
                    MessageBox.Show("Не указан путь сохранения", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Сохранять.Checked = false;
                }
            }
           
        }
        //ToolStrip
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Разработанно студентом ВолГУ\nКурдюмов Артем, ИСТб-201\n© 06.05.2022 v.1.1.1", "Информация");
        }
        private void открытьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"; //формат загружаемого файла

            if (open_dialog.ShowDialog() == DialogResult.OK)
                try
                {
                    image = new Bitmap(open_dialog.FileName);         
                    pictureBox1.Image = image;
                    DialogResult dialogResult = MessageBox.Show("Изображение загруженно. Продолжить?", "Загрузка изображения", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                        pictureBox1.BackColor = Color.Transparent;
                    else if (dialogResult == DialogResult.No)
                    {
                        MessageBox.Show("Загрузка отменена", "Уведомление");
                        pictureBox1.Image = null;
                        pictureBox1.BackgroundImage = null;
                        pictureBox2.BackgroundImage = null;

                    }
                }
                catch
                {
                    MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            else
            {
                MessageBox.Show("Загрузка отменена", "Уведомление");
                pictureBox1.Image = null;
                pictureBox1.BackgroundImage = null;
                pictureBox2.BackgroundImage = null;
            }
        }
        public void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pictureBox2.Image != null) // если изображение в pictureBox2 имеется
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Сохранить картинку как...";
                sfd.OverwritePrompt = true; // показывать ли "Перезаписать файл" если пользователь указывает имя файла, который уже существует
                sfd.CheckPathExists = true; // отображает ли диалоговое окно предупреждение, если пользователь указывает путь, который не существует
                                            // фильтр форматов файлов
                sfd.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                sfd.ShowHelp = true; // отображается ли кнопка Справка в диалоговом окне
                                     // если в диалоге была нажата кнопка ОК
                if (sfd.ShowDialog() == DialogResult.OK)
                    try
                    {
                        // сохраняем изображение
                        pictureBox2.Image.Save(sfd.FileName);
                    }
                    catch // в случае ошибки выводим MessageBox
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
            else
                MessageBox.Show("Нет доступных изображений для сохранения", "Ошибка");
        }
        public void задатьПутьToolStripMenuItem_Click(object sender, EventArgs e)
        {  
            using (var path_dial = new FolderBrowserDialog())
            {
                if (path_dial.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // Сохранение выбранного пути;
                    path = path_dial.SelectedPath + (@"\");
                    MessageBox.Show("Путь сохранен", "Уведомление");
                }
                else
                {
                    MessageBox.Show("Операция отменена", "Ошибка");
                }
            }
        }
        private void инструкцияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("                              Добро пожаловать в приложение 'Фото редактор'! \n\n 1. Для открытия изображения воспользуйтесь кнопкой 'Открыть.' \n\n 2. Для обработки изображения выбирите любой фильтр из ниже предложанных. \n\n 3. Для сохранения выбирие кнопку 'Сохранить'. \n\n 4. Чтобы изображения сохранялись автоматически после использования \n\n    фильтра, выбирите кнопку 'Сохранить' -> 'Указать путь'. Далее активируйте \n    чекбокс 'Сохранять'. ", "Инструкция");
        }


    }

}
