using System.Windows.Forms;
using System.Globalization;

namespace GaussJacobiCurs_WinForms
{
    public partial class Form1 : Form
    { // Объявление массивов данных
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
            // Открытие окна выбора файла для чтения
            OpenFileDialog ofdlg = new OpenFileDialog();
            ofdlg.Title = "Выберите файл для открытия";
            ofdlg.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            ofdlg.FilterIndex = 1;
            ofdlg.RestoreDirectory = true;

            if (ofdlg.ShowDialog() == DialogResult.OK)
            {
                openfileTxtBox.Text = ofdlg.FileName;
            }
        }
        private void OpenFileBtn_Click(object sender, EventArgs e)
        { 
            // Чтение текста файла, занесение в equationsTxtBox
            filePath = openfileTxtBox.Text;
            if (File.Exists(filePath)) 
                equationsTxtBox.Text = File.ReadAllText(filePath);
            else
                MessageBox.Show("Файл не выбран или не существует");

            // Сброс текста в надписях
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
            // Объявление размеров массивов
            B = new double[b.Length, b.Length];
            E = new double[b.Length, b.Length];
            D = new double[b.Length, b.Length];
            L = new double[b.Length, b.Length];
            g = new double[b.Length];

            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j < b.Length; j++)
                {
                    E[i, j] = 0;
                    D[i, j] = 0;
                    if (i == j)
                    {
                        E[i, j] = 1; // Заполнение единичной матрицы,
                        D[i, j] = A[i, j]; // диагональной матрицы
                    }
                    if (i > j)
                        L[i, j] = A[i, j]; // Заполнение нижнетреугольной матрциы
                }
                g[i] = 0; // Заполнение вектора g,
                initialGuess[i] = 0; // изначальных предположений о корне
            }

            int iterations = 10000; // Максимальное количество итераций
            double tolerance = 0.00001; // Допустимая погрешность

            // Создание экземпляра класса LinearEquationSolver
            LinearEquationSolver solver = // Создание объекта класса LinearEquationSolver
                new LinearEquationSolver(A, B, E, D, L, 
                                        b, g, initialGuess, 
                                        iterations, tolerance, 
                                        GaussNormLbl, JacobiNormLbl, 
                                        gaussIterLbl, jacobiIterLbl);

            // Решение СЛАУ методом Якоби
            double[] jacobiSolution = solver.JacobiMethod();

            // Решение СЛАУ методом Гаусса-Зейделя
            double[] gaussSeidelSolution = solver.GaussSeidelMethod();

            // Вывод решения при помощи метода Якоби
            DisplaySolution(jacobiLstBox, jacobiSolution, numEquations);
            // Вывод решения при помощи метода Гаусса-Зейделя
            DisplaySolution(gaussSeidelLstBox, gaussSeidelSolution, numEquations);
        }
        // Вывод решения на экран
        private void DisplaySolution(ListBox listBox, double[] solution, int n)
        {
            listBox.Items.Clear();
            string[] sln_ = ["Решение", "не", "найдено"];
            if (solution != null)
            {
                for (int i = 0; i < n; i++)
                    listBox.Items.Add(string.Format("x{0} = {1,14:E6}", i + 1, solution[i]));
            }
            else
            {
                for (int i = 0; i < 3; i++)
                    listBox.Items.Add(sln_[i]);
            }
        }
    }
    class LinearEquationSolver
    {
        private double[,] A;    // Матрица коэффициентов
        private double[] b;     // Вектор констант
        private double[,] B;    // Итерационная матрица
        private double[,] E;    // Единичная матрица
        private double[,] D;    // Диагональная матрица
        private double[,] L;    // Нижнетреугольная матрица
        private double[] g;     // Вектор смещения
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

        // Решение СЛАУ методом Якоби
        public double[] JacobiMethod()
        {
            int n = b.Length; // Размер системы уравнений
            double[] currentSolution = new double[n]; // Текущее решение
            double[] nextSolution = new double[n]; // Новое решение
            double[,] D_1 = new double[n, n];
            // Инициализация начальным приближением
            Array.Copy(initialGuess, currentSolution, n);
            // Вычисление обратной матрицы D
            Array.Copy(D, D_1, n * n);
            D_1 = InverseMatrix(D_1);
            // Вычисление матрицы B, вектора g
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
                    // Проверка решения
                    if (nextSolution[i] == Double.NegativeInfinity ||
                        nextSolution[i] == Double.PositiveInfinity ||
                        Double.IsNaN(nextSolution[i]))
                    {
                        jacobiIterLbl.Text = $"Итераций:\r\n{iter + 1}\r\nМетод не сошелся!";
                        return null;
                    }
                }
                // Проверка сходимости
                if (IsConverged(currentSolution, nextSolution))
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
        // Решение СЛАУ методом Гаусса-Зейделя
        public double[] GaussSeidelMethod()
        {
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
                    // Проверка решения
                    if (nextSolution[i] == Double.NegativeInfinity || 
                        nextSolution[i] == Double.PositiveInfinity || 
                        Double.IsNaN(nextSolution[i]))
                    {
                        gaussIterLbl.Text = $"Итераций:\r\n{iter + 1}\r\nМетод не сошелся!";
                        return null;
                    }
                }
                // Проверка сходимости
                if (IsConverged(currentSolution, nextSolution))
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
        // Подсчет спектрального радиуса матрицы
        static double ComputeSpectralRadius(double[,] matrix, int maxIterations, double tolerance)
        {
            int n = matrix.GetLength(0);
            double[] b = new double[n];
            double[] bNext = new double[n];

            // Инициализация вектора
            for (int i = 0; i < n; i++)
                b[i] = 1.0;

            double lambda = 0, lambdaOld = 0;

            for (int iter = 0; iter < maxIterations; iter++)
            {
                // Умножение матрицы на вектор
                for (int i = 0; i < n; i++)
                {
                    bNext[i] = 0;
                    for (int j = 0; j < n; j++)
                        bNext[i] += matrix[i, j] * b[j];
                }

                // Нахождение новой lambda (собственного значения) как наибольшей по модулю величины в bNext
                lambda = 0;
                for (int i = 0; i < n; i++)
                {
                    if (Math.Abs(bNext[i]) > lambda)
                        lambda = Math.Abs(bNext[i]);
                }

                // Нормализация bNext с использованием lambda
                for (int i = 0; i < n; i++)
                    bNext[i] /= lambda;

                // Проверка на сходимость
                if (Math.Abs(lambda - lambdaOld) < tolerance)
                    break;

                lambdaOld = lambda;
                Array.Copy(bNext, b, n);
            }

            return lambda;
        }
        // Нахождение обратной матрицы
        private double[,] InverseMatrix(double[,] matrix)
        {
            int n = matrix.GetLength(0); // Получаем размерность матрицы
            double[,] augmentedMatrix = AugmentMatrix(matrix); // Создаем расширенную матрицу

            // Применяем метод Гаусса-Жордана для приведения к ступенчатому виду
            for (int i = 0; i < n; i++)
            {
                // Поиск ведущей строки
                int pivotRow = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (Math.Abs(augmentedMatrix[j, i]) > Math.Abs(augmentedMatrix[pivotRow, i]))
                        pivotRow = j;
                }

                // Меняем строки местами, если ведущая строка не текущая
                if (pivotRow != i)
                {
                    for (int k = 0; k < 2 * n; k++)
                    {
                        double temp = augmentedMatrix[i, k];
                        augmentedMatrix[i, k] = augmentedMatrix[pivotRow, k];
                        augmentedMatrix[pivotRow, k] = temp;
                    }
                }

                // Нормализация ведущей строки
                double pivotValue = augmentedMatrix[i, i];
                for (int k = 0; k < 2 * n; k++)
                {
                    augmentedMatrix[i, k] /= pivotValue;
                }

                // Устраняем элементы над и под ведущим элементом
                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        double factor = augmentedMatrix[j, i];
                        for (int k = 0; k < 2 * n; k++)
                            augmentedMatrix[j, k] -= factor * augmentedMatrix[i, k];
                    }
                }
            }

            // Извлечение обратной матрицы из расширенной матрицы
            double[,] inverse = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    inverse[i, j] = augmentedMatrix[i, j + n];
            }

            return inverse; // Возвращаем обратную матрицу
        }

        private double[,] AugmentMatrix(double[,] matrix)
        {
            int n = matrix.GetLength(0); // Получаем размерность матрицы
            double[,] augmentedMatrix = new double[n, 2 * n]; // Создаем расширенную матрицу размером n x 2n
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    augmentedMatrix[i, j] = matrix[i, j]; // Копируем исходную матрицу
                augmentedMatrix[i, i + n] = 1; // Добавляем единичную матрицу справа от исходной
            }
            return augmentedMatrix; // Возвращаем расширенную матрицу
        }


        // Метод проверки сходимости
        private bool IsConverged(double[] previousSolution, double[] currentSolution)
        {
            int n = previousSolution.Length;
            for (int i = 0; i < n; i++)
            {
                if (Math.Abs(currentSolution[i] - previousSolution[i]) > tolerance)
                    return false;
            }
            return true;
        }
    }
}
