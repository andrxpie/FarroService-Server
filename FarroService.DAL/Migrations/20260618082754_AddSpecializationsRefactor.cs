using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarroService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecializationsRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MasterSpecialization",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "SpecializationId",
                table: "Services",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterSpecializations",
                columns: table => new
                {
                    MastersId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecializationsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterSpecializations", x => new { x.MastersId, x.SpecializationsId });
                    table.ForeignKey(
                        name: "FK_MasterSpecializations_AspNetUsers_MastersId",
                        column: x => x.MastersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterSpecializations_Specializations_SpecializationsId",
                        column: x => x.SpecializationsId,
                        principalTable: "Specializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_SpecializationId",
                table: "Services",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterSpecializations_SpecializationsId",
                table: "MasterSpecializations",
                column: "SpecializationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Specializations_SpecializationId",
                table: "Services",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Specializations_SpecializationId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "MasterSpecializations");

            migrationBuilder.DropTable(
                name: "Specializations");

            migrationBuilder.DropIndex(
                name: "IX_Services_SpecializationId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "SpecializationId",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "MasterSpecialization",
                table: "AspNetUsers",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);
        }
    }
}
