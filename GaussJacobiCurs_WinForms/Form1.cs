using System.Windows.Forms;
using System.Globalization;

namespace GaussJacobiCurs_WinForms
{
    public partial class Form1 : Form
    {
        private double[,] A;
        private double[,] B;
        private double[,] D;
        private double[,] E;
        private double[,] L;
        private double[] b;
        private double[] g;
        private string filePath;
        public Form1()
        {
            InitializeComponent();


        }

        private void ChooseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdlg = new OpenFileDialog();
            ofdlg.Title = "Выберите файл для открытия";
            ofdlg.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            ofdlg.FilterIndex = 1;
            ofdlg.RestoreDirectory = true;
            if (ofdlg.ShowDialog() == DialogResult.OK)
            {
                filePath = ofdlg.FileName;
                openfileTxtBox.Text = filePath;
            }
        }
        private void OpenFileBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists(filePath))
            {
                equationsTxtBox.Text = File.ReadAllText(filePath);
            }
            else
            {
                MessageBox.Show("Файл не выбран или не существует");
            }
            GaussNormLbl.Text = "Норма матрицы B \r\nравна:";
            JacobiNormLbl.Text = "Норма матрицы B \r\nравна:";
            jacobiIterLbl.Text = $"Итераций:\r\n";
            gaussIterLbl.Text = $"Итераций:\r\n";
            gaussSeidelLstBox.Items.Clear();
            jacobiLstBox.Items.Clear();
        }
        private void calcBtn_Click(object sender, EventArgs e)
        {
            // Получение содержимого TextBox
            string textBoxContent = equationsTxtBox.Text;

            // Разделение содержимого TextBox на строки
            string[] lines = textBoxContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // Подсчет количества строк и их содержимого
            int numEquations = lines.Length;

            A = new double[numEquations, numEquations]; // Создание массива коэффициентов
            b = new double[numEquations]; // Создание массива констант
            double[] initialGuess = new double[numEquations]; // Начальное приближение

            // Заполнение массива коэффициентов и массива констант
            for (int i = 0; i < numEquations; i++)
            {
                string[] coefficients = lines[i].Split(' ');
                for (int j = 0; j < numEquations; j++)
                {
                    A[i, j] = double.Parse(coefficients[j], CultureInfo.InvariantCulture);
                }
                b[i] = double.Parse(coefficients[numEquations]); // последний элемент в строке
            }
            double[,] B = new double[b.Length, b.Length];
            double[,] E = new double[b.Length, b.Length];
            double[,] D = new double[b.Length, b.Length];
            double[,] L = new double[b.Length, b.Length];
            double[] g = new double[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    E[i, j] = 0;
                    D[i, j] = 0;
                    if (i == j)
                    {
                        E[i, j] = 1;
                        D[i, j] = A[i, j];
                    }
                    if (i > j)
                        L[i, j] = A[i, j];
                }
                g[i] = 0;
                initialGuess[i] = 0;
            }

            int iterations = 10000; // Максимальное количество итераций
            double tolerance = 0.00001; // Допустимая погрешность

            // Создание экземпляра класса LinearEquationSolver
            LinearEquationSolver solver = 
                new LinearEquationSolver(A, B, E, D, L, 
                                        b, g, initialGuess, 
                                        iterations, tolerance, 
                                        GaussNormLbl, JacobiNormLbl, gaussIterLbl, jacobiIterLbl);

            // Решение СЛАУ методом Якоби
            double[] jacobiSolution = solver.JacobiMethod();

            // Решение СЛАУ методом Гаусса-Зейделя
            double[] gaussSeidelSolution = solver.GaussSeidelMethod();

            ExportSolution(jacobiSolution, gaussSeidelSolution, numEquations);
        }
        private void ExportSolution(double[] jacobiSolution, double[] gaussSeidelSolution, int numEquations)
        {
            string[] sln_ = ["Решение", "не", "найдено"];
            jacobiLstBox.Items.Clear();
            gaussSeidelLstBox.Items.Clear();
            if (jacobiSolution != null)
            {
                for (int i = 0; i < numEquations; i++)
                    jacobiLstBox.Items.Add(string.Format("x{0} = {1,14:E6}", i + 1, jacobiSolution[i]));
                
            }
            else
            {
                for (int i = 0; i < 3; i++)
                    jacobiLstBox.Items.Add(sln_[i]);
            }
            if (gaussSeidelSolution != null)
            {
                for (int i = 0; i < numEquations; i++)
                    gaussSeidelLstBox.Items.Add(string.Format("x{0} = {1,14:E6}", i + 1, gaussSeidelSolution[i]));
            }
            else
            {
                for (int i = 0; i < 3; i++)
                    gaussSeidelLstBox.Items.Add(sln_[i]);
            }
        }
    }
    class LinearEquationSolver
    {
        private double[,] A; // Матрица коэффициентов
        private double[] b;     // Вектор констант
        private double[,] B;
        private double[,] E;
        private double[,] D;
        private double[,] L;
        private double[] g;
        private double[] initialGuess;  // Начальное приближение
        private int iterations;         // Количество итераций
        private double tolerance;       // Допустимая погрешность
        private Label GaussNormLbl;
        private Label JacobiNormLbl;
        private Label gaussIterLbl;
        private Label jacobiIterLbl;
        // Конструктор класса
        public LinearEquationSolver(double[,] A, double[,] B, double[,] E, double[,] D, double[,] L, 
                                    double[] b, double[] g, double[] initialGuess, 
                                    int iterations, double tolerance, 
                                    Label GaussNormLbl, Label JacobiNormLbl, Label gaussIterLbl, Label jacobiIterLbl)
        {
            this.A = A;
            this.B = B;
            this.E = E;
            this.D = D;
            this.L = L;
            this.b = b;
            this.g = g;
            this.initialGuess = initialGuess;
            this.iterations = iterations;
            this.tolerance = tolerance;
            this.GaussNormLbl = GaussNormLbl;
            this.JacobiNormLbl = JacobiNormLbl;
            this.gaussIterLbl = gaussIterLbl;
            this.jacobiIterLbl = jacobiIterLbl;
        }

        // Метод решения СЛАУ методом Якоби
        public double[] JacobiMethod()
        {
            bool check = false;
            int n = b.Length; // Размер системы уравнений
            double[] currentSolution = new double[n]; // Текущее решение
            double[] nextSolution = new double[n]; // Новое решение
            double[,] D_1 = new double[n, n];
            // Инициализация начальным приближением
            Array.Copy(initialGuess, currentSolution, n);
            Array.Copy(D, D_1, n * n);
            D_1 = InverseMatrix(D_1);
            for (int i = 0; i < n; i++)
            {
                g[i] = 0;
                for (int j = 0; j < n; j++)
                {
                    B[i, j] = E[i, j];
                    for (int k = 0; k < n; k++)
                        B[i, j] -= D_1[i, k] * A[k, j];
                    g[i] += D_1[i, j] * b[j];
                }
            }
            JacobiNormLbl.Text = $"Норма матрицы B \r\nравна: {ComputeSpectralRadius(B, iterations, tolerance),14:E6}";
            for (int iter = 0; iter < iterations; iter++)
            { // Итерации
                for (int i = 0; i < n; i++)
                { // Обновление каждой переменной
                    nextSolution[i] = g[i];
                    for (int j = 0; j < n; j++)
                        nextSolution[i] += B[i, j] * currentSolution[j];
                }
                foreach (var solution in nextSolution)
                {
                    if (solution == Double.NegativeInfinity || Double.IsNaN(solution))
                    {
                        check = true;
                        jacobiIterLbl.Text = $"Итераций:\r\n{iter + 1}\r\nМетод не сошелся!";
                        return null;
                    }
                }
                // Проверка сходимости
                if (IsConverged(currentSolution, nextSolution) && !check)
                {
                    jacobiIterLbl.Text = $"Итераций:\r\n{iter + 1}";
                    return nextSolution;
                }

                // Переход к следующему шагу итерации
                Array.Copy(nextSolution, currentSolution, n);
            }

            MessageBox.Show("Метод Якоби не сошелся за заданное число итераций.");
            jacobiIterLbl.Text = $"Итераций:\r\n{iterations}\r\nМетод не сошелся!";
            return null;
        }
        // Метод решения СЛАУ методом Гаусса-Зейделя
        public double[] GaussSeidelMethod()
        {
            bool check = false;
            int n = b.Length; // Размер системы уравнений
            double[] currentSolution = new double[n]; // Текущее решение
            double[] nextSolution = new double[n]; // Новое решение
            double[,] D_2 = new double[n, n];
            // Инициализация начальным приближением
            Array.Copy(initialGuess, currentSolution, n);
            Array.Copy(D, D_2, n * n);
            for (int i = 0; i < n; i++)
            {
                g[i] = 0;
                for (int j = 0; j < n; j++)
                {
                    D_2[i, j] += L[i, j];
                }
            }
            D_2 = InverseMatrix(D_2);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    B[i, j] = E[i, j];
                    for (int k = 0; k < n; k++)
                        B[i, j] -= D_2[i, k] * A[k, j];
                    g[i] += D_2[i, j] * b[j];
                }
            }
            GaussNormLbl.Text = $"Норма матрицы B \r\nравна: {ComputeSpectralRadius(B, iterations, tolerance), 14:E6}";
            for (int iter = 0; iter < iterations; iter++)
            { // Итерации
                for (int i = 0; i < n; i++)
                { // Обновление каждой переменной
                    nextSolution[i] = g[i];
                    for (int j = 0; j < n; j++)
                        nextSolution[i] += B[i, j] * currentSolution[j];
                }
                foreach (var solution in nextSolution)
                {
                    if (solution == Double.NegativeInfinity || Double.IsNaN(solution))
                    {
                        check = true;
                        gaussIterLbl.Text = $"Итераций:\r\n{iter + 1}\r\nМетод не сошелся!";
                        return null;
                    }
                }
                // Проверка сходимости
                if (IsConverged(currentSolution, nextSolution) && !check)
                {

                    gaussIterLbl.Text = $"Итераций:\r\n{iter + 1}";
                    return nextSolution;
                }

                // Переход к следующему шагу итерации
                Array.Copy(nextSolution, currentSolution, n);
            }

            gaussIterLbl.Text = $"Итераций:\r\n{iterations}\r\nМетод не сошелся!";
            return null;
        }
        static double ComputeSpectralRadius(double[,] matrix, int maxIterations, double tolerance)
        {
            int n = matrix.GetLength(0);
            double[] b = new double[n];
            double[] bNext = new double[n];

            // Инициализация вектора
            for (int i = 0; i < n; i++)
            {
                b[i] = 1.0;
            }

            double lambda = 0, lambdaOld = 0;

            for (int iter = 0; iter < maxIterations; iter++)
            {
                // Умножение матрицы на вектор
                for (int i = 0; i < n; i++)
                {
                    bNext[i] = 0;
                    for (int j = 0; j < n; j++)
                    {
                        bNext[i] += matrix[i, j] * b[j];
                    }
                }

                // Нахождение новой lambda (собственного значения) как наибольшей по модулю величины в bNext
                lambda = 0;
                for (int i = 0; i < n; i++)
                {
                    if (Math.Abs(bNext[i]) > lambda)
                    {
                        lambda = Math.Abs(bNext[i]);
                    }
                }

                // Нормализация bNext lambdой
                for (int i = 0; i < n; i++)
                {
                    bNext[i] /= lambda;
                }

                // Проверка на сходимость
                if (Math.Abs(lambda - lambdaOld) < tolerance)
                {
                    break;
                }

                lambdaOld = lambda;
                Array.Copy(bNext, b, n);
            }

            return lambda;
        }
        private double[,] InverseMatrix(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[,] augmentedMatrix = AugmentMatrix(matrix);

            // Applying Gaussian elimination
            for (int i = 0; i < n; i++)
            {
                // Finding pivot row
                int pivotRow = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (Math.Abs(augmentedMatrix[j, i]) > Math.Abs(augmentedMatrix[pivotRow, i]))
                    {
                        pivotRow = j;
                    }
                }

                // Swapping rows if necessary
                if (pivotRow != i)
                {
                    for (int k = 0; k < 2 * n; k++)
                    {
                        double temp = augmentedMatrix[i, k];
                        augmentedMatrix[i, k] = augmentedMatrix[pivotRow, k];
                        augmentedMatrix[pivotRow, k] = temp;
                    }
                }
                // Normalizing pivot row
                double pivotValue = augmentedMatrix[i, i];
                for (int k = 0; k < 2 * n; k++)
                {
                    augmentedMatrix[i, k] /= pivotValue;
                }
                // Elimination
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        double factor = augmentedMatrix[j, i];
                        for (int k = 0; k < 2 * n; k++)
                        {
                            augmentedMatrix[j, k] -= factor * augmentedMatrix[i, k];
                        }
                    }
                }
            }
            // Extracting inverse matrix
            double[,] inverse = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    inverse[i, j] = augmentedMatrix[i, j + n];
                }
            }

            return inverse;
        }
        private double[,] AugmentMatrix(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[,] augmentedMatrix = new double[n, 2 * n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmentedMatrix[i, j] = matrix[i, j];
                }
                augmentedMatrix[i, i + n] = 1;
            }
            return augmentedMatrix;
        }

        // Метод проверки сходимости
        private bool IsConverged(double[] previousSolution, double[] currentSolution)
        {
            int n = previousSolution.Length;
            for (int i = 0; i < n; i++)
            {
                if (Math.Abs(currentSolution[i] - previousSolution[i]) > tolerance)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
