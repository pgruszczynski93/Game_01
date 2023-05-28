namespace PG.Game.Helpers {
    public class Consts {
        public const string COLLISION_LAYER_NAME = "Collisions";
        public const int ENEMIES_LEFT_TO_INCREASE_GRID_MOVEMENT_STEP = 2;
        public const int ENEMIES_IN_ROW = 5;
        public const int ENEMIES_IN_COLUMN = 3;
        public const int ENEMIES_TOTAL = 15;
        public const int SCREEN_EDGES = 4;

        public const float GRID_LEFT_EDGE = -3f;
        public const float GRID_RIGHT_EDGE = 3f;
        public const float ASTEROIDS_RESPAWN_DELAY = 1.2f;
        public const float NEW_WAVE_COOLDOWN = 1f;
        public const float ENEMYGRID_MOVEMENT_STEP_1 = 5f;
        public const float ENEMYGRID_MOVEMENT_STEP_2 = 7f;
        public const float ENEMIES_GRID_OFFSET = 0.5f;
        public const float ENEMY_MIN_SHOOT_DELAY = 0.4f;
        public const float ENEMY_MAX_SHOOT_DELAY = 1f;
        public const float END_WAVE_DELAY = 3f;
        public const float MIN_ASTEROID_Z = 10f;
        public const float MAX_ASTEROID_Z = 15f;
    }
}