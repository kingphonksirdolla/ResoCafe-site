using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResoCafe.Migrations
{
    /// <inheritdoc />
    public partial class FixStaticSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2b$12$Wfp2gNKHpg/g582i5UG5c.Fc0EKMtzQKSHDvMqlhPTdybiNAIT3cW");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$i97eOL7ocoQjIezc.qe8GeqGtqjseXaIWCtM2s/WNTTpr1T1q49yG");
        }
    }
}
