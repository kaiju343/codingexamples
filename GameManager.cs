using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public GameObject Loading;
	public GameObject Win;
    AlphaBeta ab = new AlphaBeta();
    private bool _kingDead = false;
    float timer = 0;
    Board _board;
	public bool singlePlayer = true;
	public bool playerTurn = true;
	public bool whiteTurn = true;
	bool AIisWhite = false;

	void Start ()
    {
		_board = Board.Instance;
		_board.SetupBoard();

		int tempCol = PlayerPrefs.GetInt ("Color");
		if (tempCol == 0) {
			Debug.Log ("0");
			AIisWhite = false;
		} else if (tempCol == 1) {
			Debug.Log ("1");
			AIisWhite = true;
			playerTurn = false;
			whiteTurn = false;
		}

		gameObject.GetComponent<ChangeBoard> ().SetBoard (!AIisWhite);
	}
		

	void Update ()
    {
        if (_kingDead)
        {
			Win.GetComponent<WinScreen> ().Win ();
        }
        if (!playerTurn && timer < 3)
        {
            timer += Time.deltaTime;
        }
		else if (!playerTurn && timer >= 3 && singlePlayer && whiteTurn == AIisWhite)
        {
			Debug.Log ("DO THE MOVE!");

            Move move = ab.GetMove();
            _DoAIMove(move);
            timer = 0;
        }

		if(Loading.activeSelf == playerTurn)
			Loading.SetActive (!playerTurn);


}


    void _DoAIMove(Move move)
    {
        Tile firstPosition = move.firstPosition;
        Tile secondPosition = move.secondPosition;

        if (secondPosition.CurrentPiece && secondPosition.CurrentPiece.Type == Piece.pieceType.KING)
        {
            SwapPieces(move);
            _kingDead = true;
        }
        else
        {
            SwapPieces(move);
        }
    }

    public void SwapPieces(Move move)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject o in objects)
        {
            Destroy(o);
        }

        Tile firstTile = move.firstPosition;
        Tile secondTile = move.secondPosition;

		if (firstTile != null)
			firstTile.CurrentPiece.MovePiece (new Vector3 (-move.secondPosition.Position.x, 0, move.secondPosition.Position.y));
		else
			Debug.Log ("LOST FIRST");
		
        if (secondTile.CurrentPiece != null)
        {
            if (secondTile.CurrentPiece.Type == Piece.pieceType.KING)
                _kingDead = true;
            Destroy(secondTile.CurrentPiece.gameObject);
        }
            

        secondTile.CurrentPiece = move.pieceMoved;
        firstTile.CurrentPiece = null;
        secondTile.CurrentPiece.position = secondTile.Position;
        secondTile.CurrentPiece.HasMoved = true;

		if (singlePlayer) {
			playerTurn = !playerTurn;
		}

		whiteTurn = !whiteTurn;
    }

	public void ResetBoard()
	{
		Application.LoadLevel (Application.loadedLevel);
	}


	public void ChangeDifficulty(int diff)
	{
		if (diff == 0)
			ab.maxDepth = 2;
		if (diff == 1)
			ab.maxDepth = 3;
		if (diff == 2)
			ab.maxDepth = 4;

	}
}
