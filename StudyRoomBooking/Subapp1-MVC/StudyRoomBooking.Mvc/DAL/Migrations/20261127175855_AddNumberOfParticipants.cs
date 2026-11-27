using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyRoomBooking.Mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberOfParticipants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfParticipants",
                table: "RoomBooking",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BookingParticipants",
                table: "BookingParticipant",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfParticipants",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfParticipants",
                table: "RoomBooking");

            migrationBuilder.DropColumn(
                name: "BookingParticipants",
                table: "BookingParticipant");

            migrationBuilder.DropColumn(
                name: "NumberOfParticipants",
                table: "AspNetUsers");
        }
    }
}
