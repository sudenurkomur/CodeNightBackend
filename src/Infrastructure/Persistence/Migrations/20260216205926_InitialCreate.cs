using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeNight.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "artists",
                columns: table => new
                {
                    artist_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    artist_name = table.Column<string>(type: "varchar(500)", nullable: false),
                    genre = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artists", x => x.artist_id);
                });

            migrationBuilder.CreateTable(
                name: "badges",
                columns: table => new
                {
                    badge_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    badge_name = table.Column<string>(type: "varchar(200)", nullable: false),
                    condition = table.Column<long>(type: "bigint", nullable: false),
                    level = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_badges", x => x.badge_id);
                });

            migrationBuilder.CreateTable(
                name: "challenge_decisions",
                columns: table => new
                {
                    decision_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    reason = table.Column<string>(type: "varchar(1000)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_challenge_decisions", x => x.decision_id);
                });

            migrationBuilder.CreateTable(
                name: "challenges",
                columns: table => new
                {
                    challenge_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    challenge_name = table.Column<string>(type: "varchar(500)", nullable: false),
                    challenge_type = table.Column<string>(type: "varchar(50)", nullable: false),
                    condition = table.Column<string>(type: "varchar(1000)", nullable: false),
                    reward_points = table.Column<long>(type: "bigint", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_challenges", x => x.challenge_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "varchar(200)", nullable: false),
                    surname = table.Column<string>(type: "varchar(200)", nullable: false),
                    city = table.Column<string>(type: "varchar(200)", nullable: false),
                    role = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "tracks",
                columns: table => new
                {
                    track_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    artist_id = table.Column<Guid>(type: "uuid", nullable: false),
                    track_name = table.Column<string>(type: "varchar(500)", nullable: false),
                    duration_sec = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tracks", x => x.track_id);
                    table.ForeignKey(
                        name: "FK_tracks_artists_artist_id",
                        column: x => x.artist_id,
                        principalTable: "artists",
                        principalColumn: "artist_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "badge_awards",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    badge_id = table.Column<Guid>(type: "uuid", nullable: false),
                    awarded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_badge_awards", x => new { x.user_id, x.badge_id });
                    table.ForeignKey(
                        name: "FK_badge_awards_badges_badge_id",
                        column: x => x.badge_id,
                        principalTable: "badges",
                        principalColumn: "badge_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_badge_awards_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "challenge_awards",
                columns: table => new
                {
                    award_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    decision_id = table.Column<Guid>(type: "uuid", nullable: false),
                    as_of_date = table.Column<DateOnly>(type: "date", nullable: false),
                    reward_points = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_challenge_awards", x => x.award_id);
                    table.ForeignKey(
                        name: "FK_challenge_awards_challenge_decisions_decision_id",
                        column: x => x.decision_id,
                        principalTable: "challenge_decisions",
                        principalColumn: "decision_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_challenge_awards_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    event_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    listen_minutes = table.Column<long>(type: "bigint", nullable: false),
                    unique_tracks = table.Column<long>(type: "bigint", nullable: false),
                    playlist_additions = table.Column<long>(type: "bigint", nullable: false),
                    shares = table.Column<long>(type: "bigint", nullable: false),
                    top_genre = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.event_id);
                    table.ForeignKey(
                        name: "FK_events_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    notification_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    channel = table.Column<string>(type: "varchar(50)", nullable: false),
                    message = table.Column<string>(type: "varchar(2000)", nullable: false),
                    sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.notification_id);
                    table.ForeignKey(
                        name: "FK_notifications_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "points_ledger",
                columns: table => new
                {
                    ledger_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    points_delta = table.Column<long>(type: "bigint", nullable: false),
                    source = table.Column<string>(type: "varchar(200)", nullable: false),
                    source_ref = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_points_ledger", x => x.ledger_id);
                    table.ForeignKey(
                        name: "FK_points_ledger_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_states",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    listen_minutes_today = table.Column<long>(type: "bigint", nullable: false),
                    unique_tracks_today = table.Column<long>(type: "bigint", nullable: false),
                    playlist_additions_today = table.Column<long>(type: "bigint", nullable: false),
                    shares_today = table.Column<long>(type: "bigint", nullable: false),
                    listen_minutes_7d = table.Column<long>(type: "bigint", nullable: false),
                    shares_7d = table.Column<long>(type: "bigint", nullable: false),
                    unique_tracks_7d = table.Column<long>(type: "bigint", nullable: false),
                    listen_streak_days = table.Column<long>(type: "bigint", nullable: false),
                    total_points = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_states", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_user_states_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "triggered_challenges",
                columns: table => new
                {
                    award_id = table.Column<Guid>(type: "uuid", nullable: false),
                    challenge_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_triggered_challenges", x => new { x.award_id, x.challenge_id });
                    table.ForeignKey(
                        name: "FK_triggered_challenges_challenge_awards_award_id",
                        column: x => x.award_id,
                        principalTable: "challenge_awards",
                        principalColumn: "award_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_triggered_challenges_challenges_challenge_id",
                        column: x => x.challenge_id,
                        principalTable: "challenges",
                        principalColumn: "challenge_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_badge_awards_badge_id",
                table: "badge_awards",
                column: "badge_id");

            migrationBuilder.CreateIndex(
                name: "IX_challenge_awards_decision_id",
                table: "challenge_awards",
                column: "decision_id");

            migrationBuilder.CreateIndex(
                name: "ix_challenge_awards_user_id_as_of_date",
                table: "challenge_awards",
                columns: new[] { "user_id", "as_of_date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_events_user_id_date",
                table: "events",
                columns: new[] { "user_id", "date" });

            migrationBuilder.CreateIndex(
                name: "ix_notifications_user_id_sent_at",
                table: "notifications",
                columns: new[] { "user_id", "sent_at" });

            migrationBuilder.CreateIndex(
                name: "ix_points_ledger_source_source_ref",
                table: "points_ledger",
                columns: new[] { "source", "source_ref" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_points_ledger_user_id_created_at",
                table: "points_ledger",
                columns: new[] { "user_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_tracks_artist_id",
                table: "tracks",
                column: "artist_id");

            migrationBuilder.CreateIndex(
                name: "ix_triggered_challenges_challenge_id",
                table: "triggered_challenges",
                column: "challenge_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "badge_awards");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "points_ledger");

            migrationBuilder.DropTable(
                name: "tracks");

            migrationBuilder.DropTable(
                name: "triggered_challenges");

            migrationBuilder.DropTable(
                name: "user_states");

            migrationBuilder.DropTable(
                name: "badges");

            migrationBuilder.DropTable(
                name: "artists");

            migrationBuilder.DropTable(
                name: "challenge_awards");

            migrationBuilder.DropTable(
                name: "challenges");

            migrationBuilder.DropTable(
                name: "challenge_decisions");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
