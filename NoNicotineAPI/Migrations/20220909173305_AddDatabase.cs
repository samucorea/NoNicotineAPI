using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoNicotineAPI.Migrations
{
    public partial class AddDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feeling",
                columns: table => new
                {
                    FeelingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feeling", x => x.FeelingId);
                });

            migrationBuilder.CreateTable(
                name: "Habit",
                columns: table => new
                {
                    HabitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habit", x => x.HabitId);
                });

            migrationBuilder.CreateTable(
                name: "IdentificationType",
                columns: table => new
                {
                    IdentificationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentificationType", x => x.IdentificationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "LinkRequestStatus",
                columns: table => new
                {
                    LinkRequestStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkRequestStatus", x => x.LinkRequestStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Symptom",
                columns: table => new
                {
                    SymptomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symptom", x => x.SymptomId);
                });

            migrationBuilder.CreateTable(
                name: "Therapist",
                columns: table => new
                {
                    TherapistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Identification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentificationTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Therapist", x => x.TherapistId);
                    table.ForeignKey(
                        name: "FK_Therapist_IdentificationType_IdentificationTypeId",
                        column: x => x.IdentificationTypeId,
                        principalTable: "IdentificationType",
                        principalColumn: "IdentificationTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DailyConsumption = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AffiliationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TherapistId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_Patient_Therapist_TherapistId",
                        column: x => x.TherapistId,
                        principalTable: "Therapist",
                        principalColumn: "TherapistId");
                });

            migrationBuilder.CreateTable(
                name: "Entry",
                columns: table => new
                {
                    EntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TherapistAllowed = table.Column<bool>(type: "bit", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entry", x => x.EntryId);
                    table.ForeignKey(
                        name: "FK_Entry_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinkRequest",
                columns: table => new
                {
                    LinkRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateAcceptedOrDeclined = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LinkRequestStatusId = table.Column<int>(type: "int", nullable: false),
                    TherapistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkRequest", x => x.LinkRequestId);
                    table.ForeignKey(
                        name: "FK_LinkRequest_LinkRequestStatus_LinkRequestStatusId",
                        column: x => x.LinkRequestStatusId,
                        principalTable: "LinkRequestStatus",
                        principalColumn: "LinkRequestStatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkRequest_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkRequest_Therapist_TherapistId",
                        column: x => x.TherapistId,
                        principalTable: "Therapist",
                        principalColumn: "TherapistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientHabit",
                columns: table => new
                {
                    PatientHabitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hour = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Days = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HabitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientHabit", x => x.PatientHabitId);
                    table.ForeignKey(
                        name: "FK_PatientHabit_Habit_HabitId",
                        column: x => x.HabitId,
                        principalTable: "Habit",
                        principalColumn: "HabitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientHabit_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientRelapseHistory",
                columns: table => new
                {
                    PatientRelapseHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountSaved = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientRelapseHistory", x => x.PatientRelapseHistoryId);
                    table.ForeignKey(
                        name: "FK_PatientRelapseHistory_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryFeeling",
                columns: table => new
                {
                    EntriesEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeelingsFeelingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryFeeling", x => new { x.EntriesEntryId, x.FeelingsFeelingId });
                    table.ForeignKey(
                        name: "FK_EntryFeeling_Entry_EntriesEntryId",
                        column: x => x.EntriesEntryId,
                        principalTable: "Entry",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntryFeeling_Feeling_FeelingsFeelingId",
                        column: x => x.FeelingsFeelingId,
                        principalTable: "Feeling",
                        principalColumn: "FeelingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntrySymptom",
                columns: table => new
                {
                    EntriesEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SymptomsSymptomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntrySymptom", x => new { x.EntriesEntryId, x.SymptomsSymptomId });
                    table.ForeignKey(
                        name: "FK_EntrySymptom_Entry_EntriesEntryId",
                        column: x => x.EntriesEntryId,
                        principalTable: "Entry",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntrySymptom_Symptom_SymptomsSymptomId",
                        column: x => x.SymptomsSymptomId,
                        principalTable: "Symptom",
                        principalColumn: "SymptomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entry_PatientId",
                table: "Entry",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryFeeling_FeelingsFeelingId",
                table: "EntryFeeling",
                column: "FeelingsFeelingId");

            migrationBuilder.CreateIndex(
                name: "IX_EntrySymptom_SymptomsSymptomId",
                table: "EntrySymptom",
                column: "SymptomsSymptomId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkRequest_LinkRequestStatusId",
                table: "LinkRequest",
                column: "LinkRequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkRequest_PatientId",
                table: "LinkRequest",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkRequest_TherapistId",
                table: "LinkRequest",
                column: "TherapistId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_TherapistId",
                table: "Patient",
                column: "TherapistId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientHabit_HabitId",
                table: "PatientHabit",
                column: "HabitId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientHabit_PatientId",
                table: "PatientHabit",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientRelapseHistory_PatientId",
                table: "PatientRelapseHistory",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Therapist_IdentificationTypeId",
                table: "Therapist",
                column: "IdentificationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntryFeeling");

            migrationBuilder.DropTable(
                name: "EntrySymptom");

            migrationBuilder.DropTable(
                name: "LinkRequest");

            migrationBuilder.DropTable(
                name: "PatientHabit");

            migrationBuilder.DropTable(
                name: "PatientRelapseHistory");

            migrationBuilder.DropTable(
                name: "Feeling");

            migrationBuilder.DropTable(
                name: "Entry");

            migrationBuilder.DropTable(
                name: "Symptom");

            migrationBuilder.DropTable(
                name: "LinkRequestStatus");

            migrationBuilder.DropTable(
                name: "Habit");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "Therapist");

            migrationBuilder.DropTable(
                name: "IdentificationType");
        }
    }
}
