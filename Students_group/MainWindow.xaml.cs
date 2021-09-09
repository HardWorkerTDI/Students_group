using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Data.Entity.Core.Objects;



namespace Students_group
{
	public partial class MainWindow : Window
	{
		Sudents_for_VorEntities studentEntities = new Sudents_for_VorEntities();
		
		private DataTable dataTable;
		private SqlConnection SqlConnection;
		private SqlDataAdapter dataAdapter;
		
		public MainWindow()
		{
			InitializeComponent();
			InitializeDataTable();
			InitializeSqlConnection();
			InitializeSqlDataAdapter();
		}

		#region Инициализация
		/// <summary>
		/// Инициализирует sql-соединение
		/// </summary>
		private void InitializeSqlConnection()
		{
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
			builder.DataSource = @"DESKTOP-GQ0VNNV\SQLEXPRESS"; // Имя SQL-сервера
			builder.InitialCatalog = "Sudents_for_Vor";         // Имя БД
			builder.IntegratedSecurity = true;                  // Проверка безопасности подключения средствами Windows
			builder.Pooling = false;                            // При закрытии соединения не помещаем его в пул 

			SqlConnection = new SqlConnection();
			SqlConnection.ConnectionString = builder.ToString();
		}// Конец функции InitializeSqlConnection

		/// <summary>
		/// Инициализирует DataTable
		/// </summary>
		private void InitializeDataTable()
		{
			dataTable = new DataTable();
		}

		/// <summary>
		/// Инициализирует SqlDataAdapter
		/// </summary>
		private void InitializeSqlDataAdapter()
		{
			dataAdapter = new SqlDataAdapter();

			try
			{
				//команда выборки записей
				dataAdapter.SelectCommand = new SqlCommand();
				dataAdapter.SelectCommand.Connection = SqlConnection;
				dataAdapter.SelectCommand.CommandType = CommandType.Text;
				dataAdapter.SelectCommand.CommandText = @"SELECT DISTINCT * FROM Student";

				//команда вставки записей
				dataAdapter.InsertCommand = new SqlCommand();
				dataAdapter.InsertCommand.Connection = SqlConnection;
				dataAdapter.InsertCommand.CommandType = CommandType.Text;
				dataAdapter.InsertCommand.CommandText = @"INSERT INTO Student(First_name, Second_name, birth_day) VALUES (@Fname, @Sname, @Bday)";
				dataAdapter.InsertCommand.Parameters.Add("@Fname", SqlDbType.NVarChar, 50, "First_name");
				dataAdapter.InsertCommand.Parameters.Add("@Sname", SqlDbType.NVarChar, 50, "Second_name");
				dataAdapter.InsertCommand.Parameters.Add("@Bday", SqlDbType.NVarChar, 4000, "birth_day");

				//команда удаления записей
				dataAdapter.DeleteCommand = new SqlCommand();
				dataAdapter.DeleteCommand.Connection = SqlConnection;
				dataAdapter.DeleteCommand.CommandType = CommandType.Text;
				dataAdapter.DeleteCommand.CommandText = @"DELETE FROM Student WHERE First_name = @Fname and  Second_name = @Sname and birth_day = @Bday";
				dataAdapter.DeleteCommand.Parameters.Add("@Fname", SqlDbType.NVarChar, 50, "First_name");
				dataAdapter.DeleteCommand.Parameters.Add("@Sname", SqlDbType.NVarChar, 50, "Second_name");
				dataAdapter.DeleteCommand.Parameters.Add("@Bday", SqlDbType.NVarChar, 4000, "birth_day");

				//команда обновления записей
				dataAdapter.UpdateCommand = new SqlCommand();
				dataAdapter.UpdateCommand.Connection = SqlConnection;
				dataAdapter.UpdateCommand.CommandType = CommandType.Text;
				dataAdapter.UpdateCommand.CommandText = @"UPDATE Student SET First_name = @Fname , Second_name = @Sname,  birth_day = @Bday WHERE First_name = @Fname, Second_name = @Sname, birth_day = @Bday";
				dataAdapter.UpdateCommand.Parameters.Add("@Fname", SqlDbType.NVarChar, 50, "First_name");
				dataAdapter.UpdateCommand.Parameters.Add("@Sname", SqlDbType.NVarChar, 50, "Second_name");
				dataAdapter.UpdateCommand.Parameters.Add("@Bday", SqlDbType.NVarChar, 4000, "birth_day");
			}
			catch(Exception ex)
			{
				MessageBox.Show("Ошибка в функции InitializeSqlDataAdapter!\n\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}// Конец функции InitializeSqlDataAdapter

		#endregion

		#region Форма

		//отображение данных в таблице при запуске
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			dataAdapter.Fill(dataTable);
			RecordsDataGrid.ItemsSource = dataTable.DefaultView;
		}

		#endregion

		#region кнопки

		//удаление
		private void DeleteButt_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (RecordsDataGrid.SelectedItems.Count == 1)
				{
					int selectedIndex = RecordsDataGrid.SelectedIndex;
					var row = dataTable.Rows[selectedIndex];
					row.Delete();

					dataAdapter.Update(dataTable);
				}

				else if (RecordsDataGrid.SelectedItems.Count > 1)
				{
					while (RecordsDataGrid.SelectedItems.Count > 0)
					{
						int selectedIndex = RecordsDataGrid.SelectedIndex;
						var row = dataTable.Rows[selectedIndex];
						row.Delete();

						dataAdapter.Update(dataTable);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void AddStudent_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(NameBox.Text) || !string.IsNullOrWhiteSpace(SecondNameBox.Text) || !string.IsNullOrWhiteSpace(BirthDayBox.Text))
				{
					var rows_Fname = dataTable.Select().Where(r => r["First_name"].ToString() == NameBox.Text);
					var rows_Sname = dataTable.Select().Where(r => r["Second_name"].ToString() == SecondNameBox.Text);
					var rows_BDay = dataTable.Select().Where(r => r["birth_day"].ToString() == BirthDayBox.Text);

					if (rows_Fname.Count() == 0 || rows_Sname.Count() == 0 || rows_Sname.Count() == 0)
					{
						DataRow row_1 = dataTable.NewRow();

						row_1["First_name"] = NameBox.Text;
						row_1["Second_name"] = SecondNameBox.Text;
						row_1["birth_day"] = BirthDayBox.Text;

						dataTable.Rows.Add(row_1);

						dataAdapter.Update(dataTable);

						//сортировка визуальной таблицы с данными
						ICollectionView cvs = CollectionViewSource.GetDefaultView(RecordsDataGrid.ItemsSource);
						if (cvs != null && cvs.CanSort == true)
						{
							cvs.SortDescriptions.Clear();
							cvs.SortDescriptions.Add(new SortDescription("First_name", ListSortDirection.Ascending));
						}
					}
					else
						MessageBox.Show("Человек с имеменем \"" + NameBox.Text + "\"" + SecondNameBox + "уже присутствует в базе данных!");

					NameBox.Clear();
					NameBox.Focus();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка в функции AddStudent_Click!\n\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		#endregion 
	}
}
