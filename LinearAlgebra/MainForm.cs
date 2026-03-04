using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace MatrixCalculator
{
    public partial class MainForm : Form
    {
        private Matrix matrixA;
        private Matrix matrixB;
        private Matrix matrixResult;
        private Vector vectorA;
        private Vector vectorB;
        private Vector vectorResult;
        private Vector vectorB_SLAE;
        private Vector vectorX;
        private SLAE slae;

        private TabControl tabControl;
        private TabPage tabMatrices;
        private TabPage tabVectors;
        private TabPage tabSLAE;

        // Контроли для вкладки "Матриці"
        private RichTextBox rtbMatrixA;
        private RichTextBox rtbMatrixB;
        private RichTextBox rtbResult;
        private TextBox txtRowsA, txtColsA, txtRowsB, txtColsB;
        private TextBox txtMinRandom, txtMaxRandom;
        private TextBox txtScalar;
        private ComboBox cmbNormType;
        private Button btnGenerateA, btnGenerateB;
        private Button btnReadA, btnReadB;
        private Button btnClearA, btnClearB;
        private Button btnAdd, btnSubtract, btnMultiply, btnMultiplyScalar;
        private Button btnTranspose, btnNorm, btnWriteResult;
        private Button btnClearResult;
        private TextBox txtMatrixFileName;
        private Label lblMatrixFileName;

        // Контроли для вкладки "Вектори"
        private RichTextBox rtbVectorA;
        private RichTextBox rtbVectorB;
        private RichTextBox rtbVectorResult;
        private TextBox txtSizeA, txtSizeB;
        private TextBox txtMinRandomVector, txtMaxRandomVector;
        private TextBox txtVectorScalar;
        private Button btnGenerateVectorA, btnGenerateVectorB;
        private Button btnReadVectorA, btnReadVectorB;
        private Button btnClearVectorA, btnClearVectorB;
        private Button btnVectorAdd, btnVectorSubtract;
        private Button btnVectorMultiplyScalar;
        private Button btnVectorDot, btnVectorNorm;
        private Button btnVectorWriteResult;
        private Button btnClearVectorResult;
        private Label lblVectorNorm;
        private Label lblVectorDot;
        private TextBox txtVectorFileName;
        private Label lblVectorFileName;

        // Контроли для вкладки "СЛАР"
        private DataGridView dgvSLAEA;
        private DataGridView dgvSLAEB;
        private DataGridView dgvSLAEX;
        private Button btnSolveSLAE;
        private Button btnRandomSLAE;
        private Button btnReadSLAE;
        private Button btnSaveSLAE;
        private Button btnClearSLAE;
        private Label lblAccuracy;

        public MainForm()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeComponent()
        {
            this.Text = "Матричний калькулятор";
            this.Size = new Size(1300, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 600);

            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Arial", 10)
            };

            tabMatrices = new TabPage("Матриці");
            tabVectors = new TabPage("Вектори");
            tabSLAE = new TabPage("СЛАР");

            CreateMatricesTab();
            CreateVectorsTab();
            CreateSLAETab();

            tabControl.TabPages.Add(tabMatrices);
            tabControl.TabPages.Add(tabVectors);
            tabControl.TabPages.Add(tabSLAE);

            this.Controls.Add(tabControl);
        }

        private void CreateMatricesTab()
        {
            TableLayoutPanel mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2,
                Padding = new Padding(10)
            };

            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));

            Panel panelA = CreateMatrixPanel("Матриця A", out rtbMatrixA, out txtRowsA, out txtColsA, out btnGenerateA, out btnReadA, out btnClearA);
            Panel panelB = CreateMatrixPanel("Матриця B", out rtbMatrixB, out txtRowsB, out txtColsB, out btnGenerateB, out btnReadB, out btnClearB);
            Panel panelResult = CreateResultPanel();
            Panel panelOperations = CreateOperationsPanel();

            mainPanel.Controls.Add(panelA, 0, 0);
            mainPanel.Controls.Add(panelB, 1, 0);
            mainPanel.Controls.Add(panelResult, 2, 0);
            mainPanel.Controls.Add(panelOperations, 0, 1);
            mainPanel.SetColumnSpan(panelOperations, 3);

            tabMatrices.Controls.Add(mainPanel);
        }

        private Panel CreateMatrixPanel(string title, out RichTextBox rtb, out TextBox txtRows, out TextBox txtCols, out Button btnGenerate, out Button btnRead, out Button btnClear)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                BackColor = Color.LightSteelBlue
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            Label lblRows = new Label { Text = "Рядки:", Location = new Point(10, 40), AutoSize = true };
            txtRows = new TextBox { Text = "3", Location = new Point(70, 37), Width = 50 };

            Label lblCols = new Label { Text = "Стовпці:", Location = new Point(130, 40), AutoSize = true };
            txtCols = new TextBox { Text = "3", Location = new Point(200, 37), Width = 50 };

            rtb = new RichTextBox
            {
                Location = new Point(10, 70),
                Size = new Size(380, 250),
                Font = new Font("Courier New", 10),
                ReadOnly = false,
                BackColor = Color.White
            };

            btnGenerate = new Button
            {
                Text = "Випадкова",
                Location = new Point(10, 330),
                Size = new Size(90, 35),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };

            btnRead = new Button
            {
                Text = "З файлу",
                Location = new Point(110, 330),
                Size = new Size(90, 35),
                BackColor = Color.LightBlue,
                FlatStyle = FlatStyle.Flat
            };

            btnClear = new Button
            {
                Text = "Очистити",
                Location = new Point(210, 330),
                Size = new Size(90, 35),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Flat
            };

            panel.Controls.AddRange(new Control[] {
                lblTitle, lblRows, txtRows, lblCols, txtCols,
                rtb, btnGenerate, btnRead, btnClear
            });

            btnGenerate.Click += BtnGenerate_Click;
            btnRead.Click += BtnRead_Click;
            btnClear.Click += BtnClear_Click;

            return panel;
        }

        private Panel CreateResultPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                BackColor = Color.LightGoldenrodYellow
            };

            Label lblTitle = new Label
            {
                Text = "Результат",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            rtbResult = new RichTextBox
            {
                Location = new Point(10, 40),
                Size = new Size(380, 290),
                Font = new Font("Courier New", 10),
                ReadOnly = true,
                BackColor = Color.LightYellow
            };

            btnClearResult = new Button
            {
                Text = "Очистити",
                Location = new Point(300, 340),
                Size = new Size(90, 30),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Flat
            };

            panel.Controls.AddRange(new Control[] { lblTitle, rtbResult, btnClearResult });
            btnClearResult.Click += (s, e) => rtbResult.Clear();

            return panel;
        }

        private Panel CreateOperationsPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                BackColor = Color.LightGray,
                AutoScroll = true
            };

            // Група основних операцій
            GroupBox gbOperations = new GroupBox
            {
                Text = "Операції над матрицями",
                Location = new Point(10, 10),
                Size = new Size(400, 90),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            btnAdd = new Button { Text = "A + B", Location = new Point(10, 25), Size = new Size(60, 30) };
            btnSubtract = new Button { Text = "A - B", Location = new Point(80, 25), Size = new Size(60, 30) };
            btnMultiply = new Button { Text = "A * B", Location = new Point(150, 25), Size = new Size(60, 30) };
            Label lblScalar = new Label { Text = "k =", Location = new Point(220, 30), AutoSize = true };
            txtScalar = new TextBox { Text = "2", Location = new Point(245, 27), Width = 50 };
            btnMultiplyScalar = new Button { Text = "A * k", Location = new Point(305, 25), Size = new Size(60, 30) };

            gbOperations.Controls.AddRange(new Control[] { btnAdd, btnSubtract, btnMultiply, lblScalar, txtScalar, btnMultiplyScalar });

            // Група додаткових операцій
            GroupBox gbAdvanced = new GroupBox
            {
                Text = "Додаткові операції",
                Location = new Point(420, 10),
                Size = new Size(380, 90),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            btnTranspose = new Button { Text = "Трансп. A", Location = new Point(10, 25), Size = new Size(80, 30) };
            cmbNormType = new ComboBox
            {
                Location = new Point(100, 30),
                Size = new Size(120, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbNormType.Items.AddRange(new object[] { "Максимальна норма", "Евклідова норма" });
            cmbNormType.SelectedIndex = 0;
            btnNorm = new Button { Text = "Норма A", Location = new Point(230, 25), Size = new Size(80, 30) };

            gbAdvanced.Controls.AddRange(new Control[] { btnTranspose, cmbNormType, btnNorm });

            // Група діапазону випадкових чисел
            GroupBox gbRandom = new GroupBox
            {
                Text = "Діапазон випадкових чисел",
                Location = new Point(810, 10),
                Size = new Size(280, 90),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            Label lblMin = new Label { Text = "Мін:", Location = new Point(10, 30), AutoSize = true };
            txtMinRandom = new TextBox { Text = "-10", Location = new Point(45, 27), Width = 60 };
            Label lblMax = new Label { Text = "Макс:", Location = new Point(115, 30), AutoSize = true };
            txtMaxRandom = new TextBox { Text = "10", Location = new Point(165, 27), Width = 60 };
            gbRandom.Controls.AddRange(new Control[] { lblMin, txtMinRandom, lblMax, txtMaxRandom });

            // Панель для збереження 
            Panel savePanel = new Panel
            {
                Location = new Point(10, 110), 
                Size = new Size(1080, 50),
                BackColor = Color.LightYellow,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblMatrixFileName = new Label
            {
                Text = "Назва файлу:",
                Location = new Point(10, 15),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            txtMatrixFileName = new TextBox
            {
                Text = "matrix_result.txt",
                Location = new Point(100, 12),
                Width = 250,
                Font = new Font("Arial", 10)
            };

            btnWriteResult = new Button
            {
                Text = "Зберегти результат",
                Location = new Point(360, 10),
                Size = new Size(150, 30),
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };

            savePanel.Controls.AddRange(new Control[] { lblMatrixFileName, txtMatrixFileName, btnWriteResult });

            panel.Controls.AddRange(new Control[] { gbOperations, gbAdvanced, gbRandom, savePanel });

            btnAdd.Click += BtnAdd_Click;
            btnSubtract.Click += BtnSubtract_Click;
            btnMultiply.Click += BtnMultiply_Click;
            btnMultiplyScalar.Click += BtnMultiplyScalar_Click;
            btnTranspose.Click += BtnTranspose_Click;
            btnNorm.Click += BtnNorm_Click;
            btnWriteResult.Click += BtnWriteResult_Click;

            return panel;
        }

        private void CreateVectorsTab()
        {
            TableLayoutPanel mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2,
                Padding = new Padding(10)
            };

            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));

            Panel panelA = CreateVectorPanel("Вектор A", out rtbVectorA, out txtSizeA, out btnGenerateVectorA, out btnReadVectorA, out btnClearVectorA);
            Panel panelB = CreateVectorPanel("Вектор B", out rtbVectorB, out txtSizeB, out btnGenerateVectorB, out btnReadVectorB, out btnClearVectorB);
            Panel panelResult = CreateVectorResultPanel();
            Panel panelOperations = CreateVectorOperationsPanel();

            mainPanel.Controls.Add(panelA, 0, 0);
            mainPanel.Controls.Add(panelB, 1, 0);
            mainPanel.Controls.Add(panelResult, 2, 0);
            mainPanel.Controls.Add(panelOperations, 0, 1);
            mainPanel.SetColumnSpan(panelOperations, 3);

            tabVectors.Controls.Add(mainPanel);
        }

        private Panel CreateVectorPanel(string title, out RichTextBox rtb, out TextBox txtSize, out Button btnGenerate, out Button btnRead, out Button btnClear)
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                BackColor = Color.LightSteelBlue
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            Label lblSize = new Label { Text = "Розмір:", Location = new Point(10, 40), AutoSize = true };
            txtSize = new TextBox { Text = "3", Location = new Point(70, 37), Width = 50 };

            rtb = new RichTextBox
            {
                Location = new Point(10, 70),
                Size = new Size(380, 250),
                Font = new Font("Courier New", 10),
                ReadOnly = false,
                BackColor = Color.White
            };

            btnGenerate = new Button
            {
                Text = "Випадковий",
                Location = new Point(10, 330),
                Size = new Size(90, 35),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };

            btnRead = new Button
            {
                Text = "З файлу",
                Location = new Point(110, 330),
                Size = new Size(90, 35),
                BackColor = Color.LightBlue,
                FlatStyle = FlatStyle.Flat
            };

            btnClear = new Button
            {
                Text = "Очистити",
                Location = new Point(210, 330),
                Size = new Size(90, 35),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Flat
            };

            panel.Controls.AddRange(new Control[] {
                lblTitle, lblSize, txtSize,
                rtb, btnGenerate, btnRead, btnClear
            });

            btnGenerate.Click += BtnGenerateVector_Click;
            btnRead.Click += BtnReadVector_Click;
            btnClear.Click += BtnClearVector_Click;

            return panel;
        }

        private Panel CreateVectorResultPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                BackColor = Color.LightGoldenrodYellow
            };

            Label lblTitle = new Label
            {
                Text = "Результат",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            rtbVectorResult = new RichTextBox
            {
                Location = new Point(10, 40),
                Size = new Size(380, 220),
                Font = new Font("Courier New", 10),
                ReadOnly = true,
                BackColor = Color.LightYellow
            };

            Panel infoPanel = new Panel
            {
                Location = new Point(10, 270),
                Size = new Size(380, 60),
                BackColor = Color.LightYellow
            };

            lblVectorNorm = new Label
            {
                Text = "Норма: ",
                Location = new Point(5, 5),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            lblVectorDot = new Label
            {
                Text = "Скалярний добуток: ",
                Location = new Point(5, 30),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            infoPanel.Controls.AddRange(new Control[] { lblVectorNorm, lblVectorDot });

            btnClearVectorResult = new Button
            {
                Text = "Очистити",
                Location = new Point(300, 340),
                Size = new Size(90, 30),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Flat
            };

            panel.Controls.AddRange(new Control[] { lblTitle, rtbVectorResult, infoPanel, btnClearVectorResult });
            btnClearVectorResult.Click += (s, e) => {
                rtbVectorResult.Clear();
                lblVectorNorm.Text = "Норма: ";
                lblVectorDot.Text = "Скалярний добуток: ";
            };

            return panel;
        }

        private Panel CreateVectorOperationsPanel()
        {
            Panel panel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                BackColor = Color.LightGray,
                AutoScroll = true
            };

            // Група операцій над векторами
            GroupBox gbOperations = new GroupBox
            {
                Text = "Операції над векторами",
                Location = new Point(10, 10),
                Size = new Size(500, 90),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            btnVectorAdd = new Button { Text = "A + B", Location = new Point(10, 25), Size = new Size(70, 30) };
            btnVectorSubtract = new Button { Text = "A - B", Location = new Point(90, 25), Size = new Size(70, 30) };
            btnVectorDot = new Button { Text = "A · B", Location = new Point(170, 25), Size = new Size(70, 30) };
            Label lblScalar = new Label { Text = "k =", Location = new Point(250, 30), AutoSize = true };
            txtVectorScalar = new TextBox { Text = "2", Location = new Point(275, 27), Width = 50 };
            btnVectorMultiplyScalar = new Button { Text = "A * k", Location = new Point(335, 25), Size = new Size(70, 30) };
            btnVectorNorm = new Button { Text = "Норма A", Location = new Point(415, 25), Size = new Size(70, 30) };

            gbOperations.Controls.AddRange(new Control[] {
                btnVectorAdd, btnVectorSubtract, btnVectorDot,
                lblScalar, txtVectorScalar, btnVectorMultiplyScalar,
                btnVectorNorm
            });

            // Група діапазону випадкових чисел
            GroupBox gbRandom = new GroupBox
            {
                Text = "Діапазон випадкових чисел",
                Location = new Point(520, 10),
                Size = new Size(280, 90),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            Label lblMin = new Label { Text = "Мін:", Location = new Point(10, 30), AutoSize = true };
            txtMinRandomVector = new TextBox { Text = "-10", Location = new Point(45, 27), Width = 60 };
            Label lblMax = new Label { Text = "Макс:", Location = new Point(115, 30), AutoSize = true };
            txtMaxRandomVector = new TextBox { Text = "10", Location = new Point(165, 27), Width = 60 };
            gbRandom.Controls.AddRange(new Control[] { lblMin, txtMinRandomVector, lblMax, txtMaxRandomVector });

            // Панель для збереження 
            Panel savePanel = new Panel
            {
                Location = new Point(10, 110),
                Size = new Size(790, 50),
                BackColor = Color.LightYellow,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblVectorFileName = new Label
            {
                Text = "Назва файлу:",
                Location = new Point(10, 15),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            txtVectorFileName = new TextBox
            {
                Text = "vector_result.txt",
                Location = new Point(100, 12),
                Width = 250,
                Font = new Font("Arial", 10)
            };

            btnVectorWriteResult = new Button
            {
                Text = "Зберегти результат",
                Location = new Point(360, 10),
                Size = new Size(150, 30),
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };

            savePanel.Controls.AddRange(new Control[] { lblVectorFileName, txtVectorFileName, btnVectorWriteResult });

            panel.Controls.AddRange(new Control[] { gbOperations, gbRandom, savePanel });

            btnVectorAdd.Click += BtnVectorAdd_Click;
            btnVectorSubtract.Click += BtnVectorSubtract_Click;
            btnVectorMultiplyScalar.Click += BtnVectorMultiplyScalar_Click;
            btnVectorDot.Click += BtnVectorDot_Click;
            btnVectorNorm.Click += BtnVectorNorm_Click;
            btnVectorWriteResult.Click += BtnVectorWriteResult_Click;

            return panel;
        }

        private void CreateSLAETab()
        {
            TableLayoutPanel mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2,
                Padding = new Padding(10)
            };

            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));

            // Панель для матриці A
            Panel panelA = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                BackColor = Color.LightSteelBlue
            };

            Label lblA = new Label
            {
                Text = "Матриця A (квадратна)",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            dgvSLAEA = new DataGridView
            {
                Location = new Point(10, 40),
                Size = new Size(380, 300),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = true,
                ColumnHeadersHeight = 30,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvSLAEA.ColumnCount = 3;
            dgvSLAEA.RowCount = 3;
            for (int i = 0; i < 3; i++)
            {
                dgvSLAEA.Columns[i].HeaderText = $"x{i + 1}";
                for (int j = 0; j < 3; j++)
                    dgvSLAEA.Rows[i].Cells[j].Value = "0";
                dgvSLAEA.Rows[i].HeaderCell.Value = $"рівняння {i + 1}";
            }

            panelA.Controls.AddRange(new Control[] { lblA, dgvSLAEA });

            // Панель для вектора B
            Panel panelB = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                BackColor = Color.LightBlue
            };

            Label lblB = new Label
            {
                Text = "Вектор B (права частина)",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            dgvSLAEB = new DataGridView
            {
                Location = new Point(10, 40),
                Size = new Size(380, 300),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = true,
                ColumnHeadersHeight = 30,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnCount = 1
            };
            dgvSLAEB.Columns[0].HeaderText = "b";
            dgvSLAEB.RowCount = 3;
            for (int i = 0; i < 3; i++)
            {
                dgvSLAEB.Rows[i].Cells[0].Value = "0";
                dgvSLAEB.Rows[i].HeaderCell.Value = $"рівняння {i + 1}";
            }

            panelB.Controls.AddRange(new Control[] { lblB, dgvSLAEB });

            // Панель для результату X
            Panel panelX = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                BackColor = Color.LightGoldenrodYellow
            };

            Label lblX = new Label
            {
                Text = "Розв'язок X",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            dgvSLAEX = new DataGridView
            {
                Location = new Point(10, 40),
                Size = new Size(380, 300),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = true,
                ColumnHeadersHeight = 30,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnCount = 1,
                BackgroundColor = Color.LightYellow
            };
            dgvSLAEX.Columns[0].HeaderText = "x";
            dgvSLAEX.RowCount = 3;
            for (int i = 0; i < 3; i++)
            {
                dgvSLAEX.Rows[i].Cells[0].Value = "";
                dgvSLAEX.Rows[i].HeaderCell.Value = $"x{i + 1}";
            }

            panelX.Controls.AddRange(new Control[] { lblX, dgvSLAEX });

            // Панель для кнопок
            Panel panelButtons = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = Color.LightGray
            };

            btnSolveSLAE = new Button
            {
                Text = "Розв'язати СЛАР",
                Location = new Point(10, 10),
                Size = new Size(150, 40),
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            btnRandomSLAE = new Button
            {
                Text = "Випадкова СЛАР",
                Location = new Point(170, 10),
                Size = new Size(150, 40),
                BackColor = Color.LightBlue,
                Font = new Font("Arial", 10)
            };

            btnReadSLAE = new Button
            {
                Text = "Зчитати з файлу",
                Location = new Point(330, 10),
                Size = new Size(150, 40),
                BackColor = Color.LightBlue,
                Font = new Font("Arial", 10)
            };

            btnSaveSLAE = new Button
            {
                Text = "Зберегти результат",
                Location = new Point(490, 10),
                Size = new Size(150, 40),
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 10)
            };

            btnClearSLAE = new Button
            {
                Text = "Очистити",
                Location = new Point(650, 10),
                Size = new Size(100, 40),
                BackColor = Color.LightCoral,
                Font = new Font("Arial", 10)
            };

            lblAccuracy = new Label
            {
                Text = "Точність: ",
                Location = new Point(760, 20),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            panelButtons.Controls.AddRange(new Control[] {
                btnSolveSLAE, btnRandomSLAE, btnReadSLAE,
                btnSaveSLAE, btnClearSLAE, lblAccuracy
            });

            mainPanel.Controls.Add(panelA, 0, 0);
            mainPanel.Controls.Add(panelB, 1, 0);
            mainPanel.Controls.Add(panelX, 2, 0);
            mainPanel.Controls.Add(panelButtons, 0, 1);
            mainPanel.SetColumnSpan(panelButtons, 3);

            tabSLAE.Controls.Add(mainPanel);

            btnSolveSLAE.Click += BtnSolveSLAE_Click;
            btnRandomSLAE.Click += BtnRandomSLAE_Click;
            btnReadSLAE.Click += BtnReadSLAE_Click;
            btnSaveSLAE.Click += BtnSaveSLAE_Click;
            btnClearSLAE.Click += BtnClearSLAE_Click;
        }

        private void InitializeData()
        {
            matrixA = new Matrix(3, 3);
            matrixB = new Matrix(3, 3);
            vectorA = new Vector(3);
            vectorB = new Vector(3);
            UpdateMatrixDisplay();
            UpdateVectorDisplay();
        }

        private void UpdateMatrixDisplay()
        {
            if (matrixA != null) rtbMatrixA.Text = matrixA.ToString();
            if (matrixB != null) rtbMatrixB.Text = matrixB.ToString();
        }

        private void UpdateVectorDisplay()
        {
            if (vectorA != null) rtbVectorA.Text = vectorA.ToString();
            if (vectorB != null) rtbVectorB.Text = vectorB.ToString();
        }

        // --- Обробники подій для матриць ---
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                bool isMatrixA = btn == btnGenerateA;
                TextBox txtRows = isMatrixA ? txtRowsA : txtRowsB;
                TextBox txtCols = isMatrixA ? txtColsA : txtColsB;

                int rows = int.Parse(txtRows.Text);
                int cols = int.Parse(txtCols.Text);
                int min = int.Parse(txtMinRandom.Text);
                int max = int.Parse(txtMaxRandom.Text);

                Matrix matrix = new Matrix(rows, cols);
                matrix.RandomInitialize(min, max);

                if (isMatrixA) matrixA = matrix;
                else matrixB = matrix;

                UpdateMatrixDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRead_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                bool isMatrixA = btn == btnReadA;

                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = isMatrixA ? "Виберіть файл з матрицею A" : "Виберіть файл з матрицею B";
                    ofd.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        Matrix matrix = Matrix.ReadFromFile(ofd.FileName);
                        if (isMatrixA)
                        {
                            matrixA = matrix;
                            txtRowsA.Text = matrix.Rows.ToString();
                            txtColsA.Text = matrix.Columns.ToString();
                        }
                        else
                        {
                            matrixB = matrix;
                            txtRowsB.Text = matrix.Rows.ToString();
                            txtColsB.Text = matrix.Columns.ToString();
                        }
                        UpdateMatrixDisplay();
                        MessageBox.Show("Матрицю успішно завантажено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка читання файлу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == btnClearA) rtbMatrixA.Clear();
            else if (btn == btnClearB) rtbMatrixB.Clear();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                matrixResult = matrixA + matrixB;
                rtbResult.Text = "A + B =\n" + matrixResult.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSubtract_Click(object sender, EventArgs e)
        {
            try
            {
                matrixResult = matrixA - matrixB;
                rtbResult.Text = "A - B =\n" + matrixResult.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnMultiply_Click(object sender, EventArgs e)
        {
            try
            {
                matrixResult = matrixA * matrixB;
                rtbResult.Text = "A * B =\n" + matrixResult.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnMultiplyScalar_Click(object sender, EventArgs e)
        {
            try
            {
                double k = double.Parse(txtScalar.Text);
                matrixResult = matrixA * k;
                rtbResult.Text = $"A * {k} =\n" + matrixResult.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTranspose_Click(object sender, EventArgs e)
        {
            try
            {
                matrixResult = matrixA.Transpose();
                rtbResult.Text = "A^T =\n" + matrixResult.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNorm_Click(object sender, EventArgs e)
        {
            try
            {
                double norm;
                if (cmbNormType.SelectedIndex == 0)
                    norm = matrixA.MaxNorm();
                else
                    norm = matrixA.FrobeniusNorm();
                rtbResult.Text = $"Норма матриці A ({cmbNormType.Text}) = {norm:F4}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnWriteResult_Click(object sender, EventArgs e)
        {
            if (matrixResult == null)
            {
                MessageBox.Show("Немає результату для збереження", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string fileName = txtMatrixFileName.Text.Trim();
                if (string.IsNullOrEmpty(fileName))
                    fileName = "matrix_result.txt";
                if (!fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    fileName += ".txt";

                matrixResult.WriteToFile(fileName);
                MessageBox.Show($"Результат успішно збережено у файл: {fileName}", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при збереженні: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Обробники подій для векторів ---
        private void BtnGenerateVector_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                bool isVectorA = btn == btnGenerateVectorA;
                TextBox txtSize = isVectorA ? txtSizeA : txtSizeB;

                int size = int.Parse(txtSize.Text);
                int min = int.Parse(txtMinRandomVector.Text);
                int max = int.Parse(txtMaxRandomVector.Text);

                Vector vector = new Vector(size);
                vector.RandomInitialize(min, max);

                if (isVectorA) vectorA = vector;
                else vectorB = vector;

                UpdateVectorDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReadVector_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                bool isVectorA = btn == btnReadVectorA;

                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = isVectorA ? "Виберіть файл з вектором A" : "Виберіть файл з вектором B";
                    ofd.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        Vector vector = Vector.ReadFromFile(ofd.FileName);
                        if (isVectorA)
                        {
                            vectorA = vector;
                            txtSizeA.Text = vector.Size.ToString();
                        }
                        else
                        {
                            vectorB = vector;
                            txtSizeB.Text = vector.Size.ToString();
                        }
                        UpdateVectorDisplay();
                        MessageBox.Show("Вектор успішно завантажено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка читання файлу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClearVector_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == btnClearVectorA)
                rtbVectorA.Clear();
            else if (btn == btnClearVectorB)
                rtbVectorB.Clear();
        }

        private void BtnVectorAdd_Click(object sender, EventArgs e)
        {
            try
            {
                vectorResult = vectorA + vectorB;
                rtbVectorResult.Text = "A + B =\n" + vectorResult.ToString();
                lblVectorNorm.Text = $"Норма результату: {vectorResult.Norm():F4}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVectorSubtract_Click(object sender, EventArgs e)
        {
            try
            {
                vectorResult = vectorA - vectorB;
                rtbVectorResult.Text = "A - B =\n" + vectorResult.ToString();
                lblVectorNorm.Text = $"Норма результату: {vectorResult.Norm():F4}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVectorMultiplyScalar_Click(object sender, EventArgs e)
        {
            try
            {
                double k = double.Parse(txtVectorScalar.Text);
                vectorResult = vectorA * k;
                rtbVectorResult.Text = $"A * {k} =\n" + vectorResult.ToString();
                lblVectorNorm.Text = $"Норма результату: {vectorResult.Norm():F4}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVectorDot_Click(object sender, EventArgs e)
        {
            try
            {
                double dot = vectorA.Dot(vectorB);
                lblVectorDot.Text = $"Скалярний добуток: {dot:F4}";
                rtbVectorResult.Text = $"A · B = {dot:F4}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVectorNorm_Click(object sender, EventArgs e)
        {
            try
            {
                double norm = vectorA.Norm();
                lblVectorNorm.Text = $"Норма вектора A: {norm:F4}";
                rtbVectorResult.Text = $"||A|| = {norm:F4}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVectorWriteResult_Click(object sender, EventArgs e)
        {
            if (vectorResult == null)
            {
                MessageBox.Show("Немає результату для збереження", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string fileName = txtVectorFileName.Text.Trim();
                if (string.IsNullOrEmpty(fileName))
                    fileName = "vector_result.txt";
                if (!fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    fileName += ".txt";

                vectorResult.WriteToFile(fileName);
                MessageBox.Show($"Результат успішно збережено у файл: {fileName}", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при збереженні: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Обробники для СЛАР ---
        private void BtnSolveSLAE_Click(object sender, EventArgs e)
        {
            try
            {
                int n = dgvSLAEA.RowCount;
                Matrix A = new Matrix(n, n);
                Vector b = new Vector(n);

                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        A[i, j] = double.TryParse(dgvSLAEA.Rows[i].Cells[j].Value?.ToString(), out double val) ? val : 0;

                for (int i = 0; i < n; i++)
                    b[i] = double.TryParse(dgvSLAEB.Rows[i].Cells[0].Value?.ToString(), out double val) ? val : 0;

                slae = new SLAE(A, b);
                slae.SolveGaussian();

                for (int i = 0; i < n; i++)
                    dgvSLAEX.Rows[i].Cells[0].Value = slae.X[i].ToString("F4");

                double accuracy = slae.CheckAccuracy();
                lblAccuracy.Text = $"Точність: {accuracy:E4}";

                Vector residual = slae.CalculateResidual();
                string message = "Розв'язок знайдено!\n\nНев'язка (похибка):\n";
                for (int i = 0; i < residual.Size; i++)
                    message += $"r{i + 1} = {residual[i]:E4}\n";
                message += $"\nМаксимальна нев'язка: {accuracy:E4}";
                MessageBox.Show(message, "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRandomSLAE_Click(object sender, EventArgs e)
        {
            try
            {
                Random rand = new Random();
                int n = 3;
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                        {
                            double val = rand.Next(-5, 6);
                            dgvSLAEA.Rows[i].Cells[j].Value = val.ToString();
                            sum += Math.Abs(val);
                        }
                    }
                    double diag = sum + rand.Next(1, 5);
                    dgvSLAEA.Rows[i].Cells[i].Value = diag.ToString();
                    dgvSLAEB.Rows[i].Cells[0].Value = rand.Next(-10, 11).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnReadSLAE_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Виберіть файл з системою рівнянь";
                    ofd.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        string[] lines = File.ReadAllLines(ofd.FileName);
                        var nonEmptyLines = new System.Collections.Generic.List<string>();
                        foreach (string line in lines)
                            if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                                nonEmptyLines.Add(line);

                        int n = nonEmptyLines.Count / 2;
                        dgvSLAEA.RowCount = n;
                        dgvSLAEA.ColumnCount = n;
                        dgvSLAEB.RowCount = n;
                        dgvSLAEX.RowCount = n;

                        for (int i = 0; i < n; i++)
                        {
                            string[] values = nonEmptyLines[i].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < n; j++)
                                dgvSLAEA.Rows[i].Cells[j].Value = values[j];
                        }
                        for (int i = 0; i < n; i++)
                            dgvSLAEB.Rows[i].Cells[0].Value = nonEmptyLines[n + i];

                        MessageBox.Show("Систему успішно завантажено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSaveSLAE_Click(object sender, EventArgs e)
        {
            if (slae == null || slae.X == null)
            {
                MessageBox.Show("Спочатку розв'яжіть СЛАР", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Зберегти результат СЛАР";
                sfd.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
                sfd.FileName = "slae_result.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName))
                        {
                            sw.WriteLine("# Розв'язок системи лінійних алгебраїчних рівнянь");
                            sw.WriteLine($"# Розмірність: {slae.X.Size}");
                            sw.WriteLine();
                            sw.WriteLine("Вектор X (розв'язок):");
                            for (int i = 0; i < slae.X.Size; i++)
                                sw.WriteLine($"x{i + 1} = {slae.X[i]:F6}");
                            sw.WriteLine();
                            sw.WriteLine($"Точність: {slae.CheckAccuracy():E6}");
                        }
                        MessageBox.Show("Результат успішно збережено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnClearSLAE_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvSLAEA.RowCount; i++)
            {
                for (int j = 0; j < dgvSLAEA.ColumnCount; j++)
                    dgvSLAEA.Rows[i].Cells[j].Value = "0";
                dgvSLAEB.Rows[i].Cells[0].Value = "0";
                dgvSLAEX.Rows[i].Cells[0].Value = "";
            }
            lblAccuracy.Text = "Точність: ";
            slae = null;
        }
    }
}