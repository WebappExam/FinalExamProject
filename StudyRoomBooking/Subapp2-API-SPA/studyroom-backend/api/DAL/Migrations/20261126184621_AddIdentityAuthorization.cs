using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyRoomBooking.Mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityAuthorization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingParticipant_AspNetUsers_UserId",
                table: "BookingParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingParticipant_RoomBooking_RoomBookingId",
                table: "BookingParticipant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingParticipant",
                table: "BookingParticipant");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "BookingParticipant",
                newName: "BookingParticipants");

            migrationBuilder.RenameIndex(
                name: "IX_BookingParticipant_UserId",
                table: "BookingParticipants",
                newName: "IX_BookingParticipants_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingParticipant_RoomBookingId",
                table: "BookingParticipants",
                newName: "IX_BookingParticipants_RoomBookingId");

            migrationBuilder.AlterColumn<string>(
                name: "StudyProgram",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingParticipants",
                table: "BookingParticipants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingParticipants_AspNetUsers_UserId",
                table: "BookingParticipants",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingParticipants_RoomBooking_RoomBookingId",
                table: "BookingParticipants",
                column: "RoomBookingId",
                principalTable: "RoomBooking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingParticipants_AspNetUsers_UserId",
                table: "BookingParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingParticipants_RoomBooking_RoomBookingId",
                table: "BookingParticipants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingParticipants",
                table: "BookingParticipants");

            migrationBuilder.RenameTable(
                name: "BookingParticipants",
                newName: "BookingParticipant");

            migrationBuilder.RenameIndex(
                name: "IX_BookingParticipants_UserId",
                table: "BookingParticipant",
                newName: "IX_BookingParticipant_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookingParticipants_RoomBookingId",
                table: "BookingParticipant",
                newName: "IX_BookingParticipant_RoomBookingId");

            migrationBuilder.AlterColumn<string>(
                name: "StudyProgram",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingParticipant",
                table: "BookingParticipant",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingParticipant_AspNetUsers_UserId",
                table: "BookingParticipant",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingParticipant_RoomBooking_RoomBookingId",
                table: "BookingParticipant",
                column: "RoomBookingId",
                principalTable: "RoomBooking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
