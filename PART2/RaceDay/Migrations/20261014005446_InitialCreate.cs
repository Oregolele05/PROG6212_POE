using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaceDay.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userRole = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userID);
                });

            migrationBuilder.CreateTable(
                name: "Organisers",
                columns: table => new
                {
                    organiserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    organiserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    organiserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    organiserPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisers", x => x.organiserID);
                    table.ForeignKey(
                        name: "FK_Organisers_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    participantID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    participantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    participantEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    participantPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.participantID);
                    table.ForeignKey(
                        name: "FK_Participants_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventGigs",
                columns: table => new
                {
                    eventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    eventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    eventDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eventLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eventDistance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eventRoute = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eventMap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    organiserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventGigs", x => x.eventID);
                    table.ForeignKey(
                        name: "FK_EventGigs_Organisers_organiserID",
                        column: x => x.organiserID,
                        principalTable: "Organisers",
                        principalColumn: "organiserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    categoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoryAge = table.Column<int>(type: "int", nullable: false),
                    categoryDistance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eventID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.categoryID);
                    table.ForeignKey(
                        name: "FK_Categories_EventGigs_eventID",
                        column: x => x.eventID,
                        principalTable: "EventGigs",
                        principalColumn: "eventID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    resultID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    participantTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    participantPosition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eventID = table.Column<int>(type: "int", nullable: false),
                    participantID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.resultID);
                    table.ForeignKey(
                        name: "FK_Results_EventGigs_eventID",
                        column: x => x.eventID,
                        principalTable: "EventGigs",
                        principalColumn: "eventID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Results_Participants_participantID",
                        column: x => x.participantID,
                        principalTable: "Participants",
                        principalColumn: "participantID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventEnrollments",
                columns: table => new
                {
                    enrollmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    enrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    eventID = table.Column<int>(type: "int", nullable: false),
                    participantID = table.Column<int>(type: "int", nullable: false),
                    categoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventEnrollments", x => x.enrollmentID);
                    table.ForeignKey(
                        name: "FK_EventEnrollments_Categories_categoryID",
                        column: x => x.categoryID,
                        principalTable: "Categories",
                        principalColumn: "categoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventEnrollments_EventGigs_eventID",
                        column: x => x.eventID,
                        principalTable: "EventGigs",
                        principalColumn: "eventID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventEnrollments_Participants_participantID",
                        column: x => x.participantID,
                        principalTable: "Participants",
                        principalColumn: "participantID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_eventID",
                table: "Categories",
                column: "eventID");

            migrationBuilder.CreateIndex(
                name: "IX_EventEnrollments_categoryID",
                table: "EventEnrollments",
                column: "categoryID");

            migrationBuilder.CreateIndex(
                name: "IX_EventEnrollments_eventID",
                table: "EventEnrollments",
                column: "eventID");

            migrationBuilder.CreateIndex(
                name: "IX_EventEnrollments_participantID",
                table: "EventEnrollments",
                column: "participantID");

            migrationBuilder.CreateIndex(
                name: "IX_EventGigs_organiserID",
                table: "EventGigs",
                column: "organiserID");

            migrationBuilder.CreateIndex(
                name: "IX_Organisers_userID",
                table: "Organisers",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_userID",
                table: "Participants",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_Results_eventID",
                table: "Results",
                column: "eventID");

            migrationBuilder.CreateIndex(
                name: "IX_Results_participantID",
                table: "Results",
                column: "participantID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventEnrollments");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "EventGigs");

            migrationBuilder.DropTable(
                name: "Organisers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
