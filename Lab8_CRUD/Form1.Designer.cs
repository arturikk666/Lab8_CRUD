namespace Lab8_CRUD
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridViewStudents = new DataGridView();
            labelName = new Label();
            textBoxName = new TextBox();
            labelAge = new Label();
            numericAge = new NumericUpDown();
            labelGroup = new Label();
            textBoxGroup = new TextBox();
            labelGrade = new Label();
            numericGrade = new NumericUpDown();
            buttonAdd = new Button();
            buttonUpdate = new Button();
            buttonDelete = new Button();
            buttonClear = new Button();
            buttonExport = new Button();
            buttonCancel = new Button();
            labelStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewStudents).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericAge).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericGrade).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewStudents
            // 
            dataGridViewStudents.AllowUserToAddRows = false;
            dataGridViewStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewStudents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewStudents.Location = new Point(407, 30);
            dataGridViewStudents.Name = "dataGridViewStudents";
            dataGridViewStudents.ReadOnly = true;
            dataGridViewStudents.RowHeadersWidth = 51;
            dataGridViewStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewStudents.Size = new Size(461, 251);
            dataGridViewStudents.TabIndex = 0;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(25, 19);
            labelName.Name = "labelName";
            labelName.Size = new Size(45, 20);
            labelName.TabIndex = 1;
            labelName.Text = "ФИО:";
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(101, 19);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(152, 27);
            textBoxName.TabIndex = 2;
            // 
            // labelAge
            // 
            labelAge.AutoSize = true;
            labelAge.Location = new Point(19, 158);
            labelAge.Name = "labelAge";
            labelAge.Size = new Size(67, 20);
            labelAge.TabIndex = 3;
            labelAge.Text = "Возраст:";
            // 
            // numericAge
            // 
            numericAge.Location = new Point(96, 156);
            numericAge.Minimum = new decimal(new int[] { 15, 0, 0, 0 });
            numericAge.Name = "numericAge";
            numericAge.Size = new Size(157, 27);
            numericAge.TabIndex = 4;
            numericAge.Value = new decimal(new int[] { 15, 0, 0, 0 });
            // 
            // labelGroup
            // 
            labelGroup.AutoSize = true;
            labelGroup.Location = new Point(25, 86);
            labelGroup.Name = "labelGroup";
            labelGroup.Size = new Size(61, 20);
            labelGroup.TabIndex = 5;
            labelGroup.Text = "Группа:";
            // 
            // textBoxGroup
            // 
            textBoxGroup.Location = new Point(101, 86);
            textBoxGroup.Name = "textBoxGroup";
            textBoxGroup.Size = new Size(152, 27);
            textBoxGroup.TabIndex = 6;
            // 
            // labelGrade
            // 
            labelGrade.AutoSize = true;
            labelGrade.Location = new Point(63, 223);
            labelGrade.Name = "labelGrade";
            labelGrade.Size = new Size(110, 20);
            labelGrade.TabIndex = 7;
            labelGrade.Text = "Средний балл:";
            // 
            // numericGrade
            // 
            numericGrade.DecimalPlaces = 2;
            numericGrade.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            numericGrade.Location = new Point(19, 268);
            numericGrade.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            numericGrade.Name = "numericGrade";
            numericGrade.Size = new Size(234, 27);
            numericGrade.TabIndex = 8;
            numericGrade.ValueChanged += numericGrade_ValueChanged;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(25, 355);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(148, 39);
            buttonAdd.TabIndex = 9;
            buttonAdd.Text = "Добавить";
            buttonAdd.UseVisualStyleBackColor = true;
            // 
            // buttonUpdate
            // 
            buttonUpdate.Location = new Point(190, 355);
            buttonUpdate.Name = "buttonUpdate";
            buttonUpdate.Size = new Size(146, 39);
            buttonUpdate.TabIndex = 10;
            buttonUpdate.Text = "Обновить";
            buttonUpdate.UseVisualStyleBackColor = true;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(353, 355);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(146, 39);
            buttonDelete.TabIndex = 11;
            buttonDelete.Text = "Удалить";
            buttonDelete.UseVisualStyleBackColor = true;
            // 
            // buttonClear
            // 
            buttonClear.Location = new Point(515, 355);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new Size(148, 39);
            buttonClear.TabIndex = 12;
            buttonClear.Text = "Очистить";
            buttonClear.UseVisualStyleBackColor = true;
            // 
            // buttonExport
            // 
            buttonExport.Location = new Point(669, 355);
            buttonExport.Name = "buttonExport";
            buttonExport.Size = new Size(148, 39);
            buttonExport.TabIndex = 13;
            buttonExport.Text = "Экспорт в CSV";
            buttonExport.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(823, 357);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(148, 37);
            buttonCancel.TabIndex = 14;
            buttonCancel.Text = "Отмена";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.Location = new Point(29, 446);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(57, 20);
            labelStatus.TabIndex = 15;
            labelStatus.Text = "Готово";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1032, 553);
            Controls.Add(labelStatus);
            Controls.Add(buttonCancel);
            Controls.Add(buttonExport);
            Controls.Add(buttonClear);
            Controls.Add(buttonDelete);
            Controls.Add(buttonUpdate);
            Controls.Add(buttonAdd);
            Controls.Add(numericGrade);
            Controls.Add(labelGrade);
            Controls.Add(textBoxGroup);
            Controls.Add(labelGroup);
            Controls.Add(numericAge);
            Controls.Add(labelAge);
            Controls.Add(textBoxName);
            Controls.Add(labelName);
            Controls.Add(dataGridViewStudents);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Лабораторная работа №8 - CRUD";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewStudents).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericAge).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericGrade).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridViewStudents;
        private Label labelName;
        private TextBox textBoxName;
        private Label labelAge;
        private NumericUpDown numericAge;
        private Label labelGroup;
        private TextBox textBoxGroup;
        private Label labelGrade;
        private NumericUpDown numericGrade;
        private Button buttonAdd;
        private Button buttonUpdate;
        private Button buttonDelete;
        private Button buttonClear;
        private Button buttonExport;
        private Button buttonCancel;
        private Label labelStatus;
    }
}
