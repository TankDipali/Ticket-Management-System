using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateAddpwdsalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "Hh8OXu/tRkTuOmk28OaGDHOhBRl35ug8TBtTsbkoNwE=", "Tiw3bXveXMlZQdnSQN8kbg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "hashedpassword");
        }
    }
}
