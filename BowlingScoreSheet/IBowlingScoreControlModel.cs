namespace BowlingScoreSheet
{
    public interface IBowlingScoreControlModel
    {
        bool GameOver { get; set; }
        void SetFrameScore(int frameNumber, int score);
        void SetTotalScore(int score);
        void SetBall(int frame, int ball, int pins);
        void Clear();
    }        
}