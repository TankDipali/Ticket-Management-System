using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "jBNfffytyc2U6m9kf+caItFf+lSniIyJqT2XTam/W4M=", "ivSJqfOfVhGCbkRMHIhYgA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "Hh8OXu/tRkTuOmk28OaGDHOhBRl35ug8TBtTsbkoNwE=", "Tiw3bXveXMlZQdnSQN8kbg==" });
        }
    }
}
