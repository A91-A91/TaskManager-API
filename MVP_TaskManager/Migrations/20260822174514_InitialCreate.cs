using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MVP_TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:public.user_role", "admin,user");

            migrationBuilder.CreateTable(
                name: "status_ref",
                columns: table => new
                {
                    id_status = table.Column<int>(type: "integer", nullable: false),
                    name_status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("status_ref_pkey", x => x.id_status);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: true),
                    login = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    reg_date = table.Column<DateOnly>(type: "date", nullable: true),
                    role = table.Column<int>(type: "user_role", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    id_task = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_user = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    id_status = table.Column<int>(type: "integer", nullable: true),
                    date_create = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tasks_pkey", x => x.id_task);
                    table.ForeignKey(
                        name: "fk_id_status",
                        column: x => x.id_status,
                        principalTable: "status_ref",
                        principalColumn: "id_status");
                    table.ForeignKey(
                        name: "fk_id_user",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tasks_id_status",
                table: "tasks",
                column: "id_status");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_id_user",
                table: "tasks",
                column: "id_user");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "status_ref");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
