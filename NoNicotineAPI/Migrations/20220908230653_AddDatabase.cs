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
                name: "Feelings",
                columns: table => new
                {
                    FeelingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feelings", x => x.FeelingId);
                });

            migrationBuilder.CreateTable(
                name: "Habits",
                columns: table => new
                {
                    HabitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habits", x => x.HabitId);
                });

            migrationBuilder.CreateTable(
                name: "IdentificationTypes",
                columns: table => new
                {
                    IdentificationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentificationTypes", x => x.IdentificationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "LinkRequestStatuses",
                columns: table => new
                {
                    LinkRequestStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkRequestStatuses", x => x.LinkRequestStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Symptoms",
                columns: table => new
                {
                    SymptomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symptoms", x => x.SymptomId);
                });

            migrationBuilder.CreateTable(
                name: "Therapists",
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
                    table.PrimaryKey("PK_Therapists", x => x.TherapistId);
                    table.ForeignKey(
                        name: "FK_Therapists_IdentificationTypes_IdentificationTypeId",
                        column: x => x.IdentificationTypeId,
                        principalTable: "IdentificationTypes",
                        principalColumn: "IdentificationTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinkRequests",
                columns: table => new
                {
                    LinkRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateAcceptedOrDeclined = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LinkRequestStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkRequests", x => x.LinkRequestId);
                    table.ForeignKey(
                        name: "FK_LinkRequests_LinkRequestStatuses_LinkRequestStatusId",
                        column: x => x.LinkRequestStatusId,
                        principalTable: "LinkRequestStatuses",
                        principalColumn: "LinkRequestStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DailyConsumption = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AffiliationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TherapistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_Patients_Therapists_TherapistId",
                        column: x => x.TherapistId,
                        principalTable: "Therapists",
                        principalColumn: "TherapistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinkRequestTherapist",
                columns: table => new
                {
                    LinkRequestsLinkRequestId = table.Column<int>(type: "int", nullable: false),
                    TherapistsTherapistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkRequestTherapist", x => new { x.LinkRequestsLinkRequestId, x.TherapistsTherapistId });
                    table.ForeignKey(
                        name: "FK_LinkRequestTherapist_LinkRequests_LinkRequestsLinkRequestId",
                        column: x => x.LinkRequestsLinkRequestId,
                        principalTable: "LinkRequests",
                        principalColumn: "LinkRequestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkRequestTherapist_Therapists_TherapistsTherapistId",
                        column: x => x.TherapistsTherapistId,
                        principalTable: "Therapists",
                        principalColumn: "TherapistId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entries",
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
                    table.PrimaryKey("PK_Entries", x => x.EntryId);
                    table.ForeignKey(
                        name: "FK_Entries_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HabitPatient",
                columns: table => new
                {
                    HabitsHabitId = table.Column<int>(type: "int", nullable: false),
                    PatientsPatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitPatient", x => new { x.HabitsHabitId, x.PatientsPatientId });
                    table.ForeignKey(
                        name: "FK_HabitPatient_Habits_HabitsHabitId",
                        column: x => x.HabitsHabitId,
                        principalTable: "Habits",
                        principalColumn: "HabitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HabitPatient_Patients_PatientsPatientId",
                        column: x => x.PatientsPatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinkRequestPatient",
                columns: table => new
                {
                    LinkRequestsLinkRequestId = table.Column<int>(type: "int", nullable: false),
                    PatientsPatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkRequestPatient", x => new { x.LinkRequestsLinkRequestId, x.PatientsPatientId });
                    table.ForeignKey(
                        name: "FK_LinkRequestPatient_LinkRequests_LinkRequestsLinkRequestId",
                        column: x => x.LinkRequestsLinkRequestId,
                        principalTable: "LinkRequests",
                        principalColumn: "LinkRequestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkRequestPatient_Patients_PatientsPatientId",
                        column: x => x.PatientsPatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientRelapseHistoric",
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
                    table.PrimaryKey("PK_PatientRelapseHistoric", x => x.PatientRelapseHistoryId);
                    table.ForeignKey(
                        name: "FK_PatientRelapseHistoric_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
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
                        name: "FK_EntryFeeling_Entries_EntriesEntryId",
                        column: x => x.EntriesEntryId,
                        principalTable: "Entries",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntryFeeling_Feelings_FeelingsFeelingId",
                        column: x => x.FeelingsFeelingId,
                        principalTable: "Feelings",
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
                        name: "FK_EntrySymptom_Entries_EntriesEntryId",
                        column: x => x.EntriesEntryId,
                        principalTable: "Entries",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntrySymptom_Symptoms_SymptomsSymptomId",
                        column: x => x.SymptomsSymptomId,
                        principalTable: "Symptoms",
                        principalColumn: "SymptomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_PatientId",
                table: "Entries",
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
                name: "IX_HabitPatient_PatientsPatientId",
                table: "HabitPatient",
                column: "PatientsPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkRequestPatient_PatientsPatientId",
                table: "LinkRequestPatient",
                column: "PatientsPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkRequests_LinkRequestStatusId",
                table: "LinkRequests",
                column: "LinkRequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkRequestTherapist_TherapistsTherapistId",
                table: "LinkRequestTherapist",
                column: "TherapistsTherapistId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientRelapseHistoric_PatientId",
                table: "PatientRelapseHistoric",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_TherapistId",
                table: "Patients",
                column: "TherapistId");

            migrationBuilder.CreateIndex(
                name: "IX_Therapists_IdentificationTypeId",
                table: "Therapists",
                column: "IdentificationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntryFeeling");

            migrationBuilder.DropTable(
                name: "EntrySymptom");

            migrationBuilder.DropTable(
                name: "HabitPatient");

            migrationBuilder.DropTable(
                name: "LinkRequestPatient");

            migrationBuilder.DropTable(
                name: "LinkRequestTherapist");

            migrationBuilder.DropTable(
                name: "PatientRelapseHistoric");

            migrationBuilder.DropTable(
                name: "Feelings");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "Symptoms");

            migrationBuilder.DropTable(
                name: "Habits");

            migrationBuilder.DropTable(
                name: "LinkRequests");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "LinkRequestStatuses");

            migrationBuilder.DropTable(
                name: "Therapists");

            migrationBuilder.DropTable(
                name: "IdentificationTypes");
        }
    }
}
