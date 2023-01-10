using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class tp2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagsPost_Posts_PostId",
                table: "TagsPost");

            migrationBuilder.DropForeignKey(
                name: "FK_TagsPost_Tags_TagId",
                table: "TagsPost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagsPost",
                table: "TagsPost");

            migrationBuilder.RenameTable(
                name: "TagsPost",
                newName: "TagPost");

            migrationBuilder.RenameIndex(
                name: "IX_TagsPost_TagId",
                table: "TagPost",
                newName: "IX_TagPost_TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagPost",
                table: "TagPost",
                columns: new[] { "PostId", "TagId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TagPost_Posts_PostId",
                table: "TagPost",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagPost_Tags_TagId",
                table: "TagPost",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagPost_Posts_PostId",
                table: "TagPost");

            migrationBuilder.DropForeignKey(
                name: "FK_TagPost_Tags_TagId",
                table: "TagPost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagPost",
                table: "TagPost");

            migrationBuilder.RenameTable(
                name: "TagPost",
                newName: "TagsPost");

            migrationBuilder.RenameIndex(
                name: "IX_TagPost_TagId",
                table: "TagsPost",
                newName: "IX_TagsPost_TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagsPost",
                table: "TagsPost",
                columns: new[] { "PostId", "TagId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TagsPost_Posts_PostId",
                table: "TagsPost",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagsPost_Tags_TagId",
                table: "TagsPost",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
