public static class PersistentGameData
{
    private static int _player1Level = 1;
    private static int _player2Level = 2;

    private static float _player1XP = 0;
    private static float _player2XP = 0;

    private static float _player1Health = 0;
    private static float _player2Health = 0;

    public static int Player1Level
    {
        get { return _player1Level; }
        set { _player1Level = value; }
    }

    public static int Player2Level
    {
        get { return _player2Level; }
        set { _player2Level = value; }
    }

    public static float Player1XP
    {
        get { return _player1XP; }
        set { _player1XP = value; }
    }

    public static float Player2XP
    {
        get { return _player2XP; }
        set { _player2XP = value; }
    }

    public static float Player1Health
    {
        get { return _player1Health; }
        set { _player1Health = value; }
    }

    public static float Player2Health
    {
        get { return _player2Health; }
        set { _player2Health = value; }
    }
}
