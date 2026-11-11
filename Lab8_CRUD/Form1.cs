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
        string connectionString = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=StudentDB;Integrated Security=True;";
        int selectedId = -1;

        public Form1()
        {
            InitializeComponent();

            dataGridViewStudents.SelectionChanged += DataGridViewStudents_SelectionChanged;
            buttonAdd.Click += ButtonAdd_Click;
            buttonUpdate.Click += ButtonUpdate_Click;
            buttonDelete.Click += ButtonDelete_Click;
            buttonClear.Click += ButtonClear_Click;
            buttonExport.Click += ButtonExport_Click;
            buttonCancel.Click += ButtonCancel_Click;
            buttonSearch.Click += buttonSearch_Click;
            buttonShowLog.Click += ButtonShowLog_Click; // Новая кнопка
        }

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
            catch (SqlException ex)
            {
                MessageBox.Show(GetSqlErrorMessage(ex), "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError("Ошибка SQL при загрузке", ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError("Общая ошибка при загрузке", ex);
            }
        }

        private void UpdateStatus(int count)
        {
            labelStatus.Text = $"Всего студентов: {count}";
        }

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

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxGroup.Text))
            {
                MessageBox.Show("Заполните ФИО и Группу!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (IsStudentExists(textBoxName.Text.Trim()))
                {
                    MessageBox.Show("Студент с таким ФИО уже существует!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
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
                Logger.LogInfo($"Добавлен студент: {textBoxName.Text}");
                LoadData();
                ClearFields();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(GetSqlErrorMessage(ex), "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError("Ошибка SQL при добавлении", ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError("Общая ошибка при добавлении", ex);
            }
        }

        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == -1) return;
            if (string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxGroup.Text)) return;

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
                MessageBox.Show("Данные обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Logger.LogInfo($"Обновлен студент: {textBoxName.Text}");
                LoadData();
                ClearFields();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(GetSqlErrorMessage(ex), "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError("Ошибка SQL при обновлении", ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == -1) return;

            DialogResult result = MessageBox.Show($"Удалить студента {textBoxName.Text}?", "Подтверждение", MessageBoxButtons.YesNo);
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
                    Logger.LogInfo($"Удален студент: {textBoxName.Text}");
                    LoadData();
                    ClearFields();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(GetSqlErrorMessage(ex), "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.LogError("Ошибка SQL при удалении", ex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
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
                MessageBox.Show("Ошибка поиска: " + ex.Message);
            }
        }

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

        private void ButtonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV|*.csv";
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
                            sb.AppendLine($"{row.Cells["Id"].Value};{row.Cells["FullName"].Value};{row.Cells["Age"].Value};{row.Cells["GroupName"].Value};{row.Cells["AverageGrade"].Value}");
                    }
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Экспорт выполнен!", "Успех");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка экспорта: " + ex.Message);
                }
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            LoadData();
            ClearFields();
        }

        // Кнопка "Показать лог"
        private void ButtonShowLog_Click(object sender, EventArgs e)
        {
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.log");
            if (File.Exists(logPath))
                System.Diagnostics.Process.Start("notepad.exe", logPath);
            else
                MessageBox.Show("Лог-файл пока пуст.");
        }

        private string GetSqlErrorMessage(SqlException ex)
        {
            switch (ex.Number)
            {
                case 4060: return "БД не найдена.";
                case 18456: return "Ошибка входа.";
                case 208: return "Таблица не найдена.";
                case 2627: return "Дубликат записи.";
                case 547: return "Нарушение ограничения CHECK.";
                case 53: return "Сервер не найден.";
                default: return $"Ошибка SQL ({ex.Number}): {ex.Message}";
            }
        }

        private bool IsStudentExists(string fullName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT COUNT(*) FROM Students WHERE FullName = @name";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", fullName);
                        return (int)command.ExecuteScalar() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Ошибка проверки дубликата", ex);
                return false;
            }
        }
    }
}