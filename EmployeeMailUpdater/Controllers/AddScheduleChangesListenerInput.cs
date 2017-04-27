namespace EmployeeMailUpdater.Controllers
{
	public class AddScheduleChangesListenerInput
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public int DaysStartFromCurrentDate { get; set; }
		public int DaysEndFromCurrentDate { get; set; }
	}
}