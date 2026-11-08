using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Lab8_CRUD
{
    public partial class Form1 : Form
    {
        // ВНИМАНИЕ! Проверьте строку подключения!
        // Если ваш сервер называется иначе (например, (localdb)\MSSQLLocalDB), измените Data Source.
        string connectionString = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=StudentDB;Integrated Security=True;";

        int selectedId = -1;

        public Form1()
        {
            InitializeComponent();

            // Привязываем события к кнопкам и таблице
            dataGridViewStudents.SelectionChanged += DataGridViewStudents_SelectionChanged;
            buttonAdd.Click += ButtonAdd_Click;
            buttonUpdate.Click += ButtonUpdate_Click;
            buttonDelete.Click += ButtonDelete_Click;
            buttonClear.Click += ButtonClear_Click;
            buttonExport.Click += ButtonExport_Click;
            buttonCancel.Click += ButtonCancel_Click;

            // Привязка новой кнопки поиска (чтобы не кликать дважды по форме)
            buttonSearch.Click += buttonSearch_Click;
        }

        // ==================== 1. ЗАГРУЗКА ДАННЫХ (READ) ====================
        // Обратите внимание: этот метод уже привязан через конструктор формы (Form1.Designer.cs)
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Students";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridViewStudents.DataSource = dt;
                    UpdateStatus(dt.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к БД: " + ex.Message + "\nПроверьте строку подключения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Счетчик студентов
        private void UpdateStatus(int count)
        {
            labelStatus.Text = $"Всего студентов: {count}";
        }

        // ==================== ВЫБОР СТРОКИ ====================
        private void DataGridViewStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewStudents.CurrentRow != null && !dataGridViewStudents.CurrentRow.IsNewRow)
            {
                DataGridViewRow row = dataGridViewStudents.CurrentRow;
                selectedId = Convert.ToInt32(row.Cells["Id"].Value);
                textBoxName.Text = row.Cells["FullName"].Value.ToString();
                numericAge.Value = Convert.ToDecimal(row.Cells["Age"].Value);
                textBoxGroup.Text = row.Cells["GroupName"].Value.ToString();

                if (row.Cells["AverageGrade"].Value != DBNull.Value)
                    numericGrade.Value = Convert.ToDecimal(row.Cells["AverageGrade"].Value);
                else
                    numericGrade.Value = 0;
            }
        }

        // ==================== 2. ДОБАВЛЕНИЕ (CREATE) ====================
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxGroup.Text))
            {
                MessageBox.Show("Заполните ФИО и Группу!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Поиск перед добавлением (проверка на дубликат ФИО)
                    string checkQuery = "SELECT COUNT(*) FROM Students WHERE FullName = @name";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@name", textBoxName.Text.Trim());
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Студент с таким ФИО уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string query = "INSERT INTO Students (FullName, Age, GroupName, AverageGrade) VALUES (@name, @age, @group, @grade)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", textBoxName.Text.Trim());
                        cmd.Parameters.AddWithValue("@age", (int)numericAge.Value);
                        cmd.Parameters.AddWithValue("@group", textBoxGroup.Text.Trim());
                        cmd.Parameters.AddWithValue("@grade", (double)numericGrade.Value);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Студент успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== 3. ОБНОВЛЕНИЕ (UPDATE) ====================
        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Выберите студента из списка для редактирования!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxGroup.Text))
            {
                MessageBox.Show("Заполните ФИО и Группу!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Students SET FullName = @name, Age = @age, GroupName = @group, AverageGrade = @grade WHERE Id = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", textBoxName.Text.Trim());
                        cmd.Parameters.AddWithValue("@age", (int)numericAge.Value);
                        cmd.Parameters.AddWithValue("@group", textBoxGroup.Text.Trim());
                        cmd.Parameters.AddWithValue("@grade", (double)numericGrade.Value);
                        cmd.Parameters.AddWithValue("@id", selectedId);

                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Данные успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== 4. УДАЛЕНИЕ (DELETE) ====================
        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Выберите студента для удаления!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Вы уверены, что хотите удалить студента {textBoxName.Text}?",
                                                  "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM Students WHERE Id = @id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", selectedId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Студент удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==================== ПОИСК (SEARCH) ====================
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            // Если поле поиска пустое — показываем всех студентов
            if (string.IsNullOrWhiteSpace(textBoxSearch.Text))
            {
                LoadData();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Ищем частичное совпадение по ФИО
                    string query = "SELECT * FROM Students WHERE FullName LIKE @name";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@name", "%" + textBoxSearch.Text.Trim() + "%");

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridViewStudents.DataSource = dt;
                    UpdateStatus(dt.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при поиске: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== ОЧИСТКА ПОЛЕЙ ====================
        private void ButtonClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            textBoxName.Clear();
            textBoxGroup.Clear();
            numericAge.Value = 15;
            numericGrade.Value = 0;
            selectedId = -1;
            dataGridViewStudents.ClearSelection();
        }

        // ==================== ЭКСПОРТ В CSV ====================
        private void ButtonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";
            sfd.FileName = "StudentsExport.csv";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Id;FullName;Age;GroupName;AverageGrade");

                    foreach (DataGridViewRow row in dataGridViewStudents.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            sb.AppendLine($"{row.Cells["Id"].Value};{row.Cells["FullName"].Value};{row.Cells["Age"].Value};{row.Cells["GroupName"].Value};{row.Cells["AverageGrade"].Value}");
                        }
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Экспорт успешно завершен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка экспорта: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==================== ОТМЕНА (ПЕРЕЗАГРУЗКА) ====================
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearFields();
            MessageBox.Show("Изменения отменены. Данные перезагружены.", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}