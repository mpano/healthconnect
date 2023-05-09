using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorAppointment.Migrations
{
    /// <inheritdoc />
    public partial class initalcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblAppointment",
                columns: table => new
                {
                    AppointmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatID = table.Column<int>(type: "int", nullable: false),
                    DocID = table.Column<int>(type: "int", nullable: false),
                    AppointmentDay = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAppointment", x => x.AppointmentID);
                });

            migrationBuilder.CreateTable(
                name: "tblDepartments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDepartments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "tblPatients",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(name: "First_Name", type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(name: "Last_Name", type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPatients", x => x.PatientId);
                });

            migrationBuilder.CreateTable(
                name: "tblUser",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserRole = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUser", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "tblWeekDays",
                columns: table => new
                {
                    WeekdaysId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Monday = table.Column<bool>(type: "bit", nullable: false),
                    Tuesday = table.Column<bool>(type: "bit", nullable: false),
                    Wednesday = table.Column<bool>(type: "bit", nullable: false),
                    Thursday = table.Column<bool>(type: "bit", nullable: false),
                    Friday = table.Column<bool>(type: "bit", nullable: false),
                    Saturday = table.Column<bool>(type: "bit", nullable: false),
                    Sunday = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblWeekDays", x => x.WeekdaysId);
                });

            migrationBuilder.CreateTable(
                name: "tblDoctor",
                columns: table => new
                {
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    DoctorFee = table.Column<int>(type: "int", nullable: false),
                    WeekdaysId = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(name: "First_Name", type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(name: "Last_Name", type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDoctor", x => x.DoctorId);
                    table.ForeignKey(
                        name: "FK_tblDoctor_tblDepartments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "tblDepartments",
                        principalColumn: "DepartmentId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblDoctor_DepartmentId",
                table: "tblDoctor",
                column: "DepartmentId");
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "tblAppointment",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblAppointment");

            migrationBuilder.DropTable(
                name: "tblDoctor");

            migrationBuilder.DropTable(
                name: "tblPatients");

            migrationBuilder.DropTable(
                name: "tblUser");

            migrationBuilder.DropTable(
                name: "tblWeekDays");

            migrationBuilder.DropTable(
                name: "tblDepartments");
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "tblAppointment");
        }
    }
}
