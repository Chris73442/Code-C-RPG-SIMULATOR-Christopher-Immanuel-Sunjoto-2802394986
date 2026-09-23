using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public enum TurnState
{
    PlayerTurn,
    EnemyTurn,
    Busy
}

public class BattleManager : MonoBehaviour
{
    [Header("Player Status")]
    public int playerHP = 100;
    public int playerMP = 20;
    public int maxPlayerHP = 100;
    public int maxPlayerMP = 20;

    [Header("Enemy Status")]
    public int enemyHP = 100;

    [Header("Animator")]
    public Animator playerAnim;
    public Animator enemyAnim;

    [Header("Turn UI")]
    public GameObject yourTurnText;
    public GameObject enemyTurnText;

    [Header("Heal Effect (Scene Object)")]
    public GameObject healEffect;

    [Header("UI Text")]
    public Text playerHPText;
    public Text playerMPText;
    public Text enemyHPText;

    private TurnState state;
    private bool battleEnded = false;

    // ================= START =================

    void Start()
    {
        
        state = TurnState.PlayerTurn;
        UpdateTurnUI();
        UpdateText();

        if (healEffect != null)
            healEffect.SetActive(false);

        if (playerAnim != null)
        {
            playerAnim.Rebind();
            playerAnim.Update(0f);
        }
    }

    // ================= UPDATE =================

    void Update()
    {
        if (battleEnded) return;
        if (state != TurnState.PlayerTurn) return;

        if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(PlayerAttack());

        if (Input.GetKeyDown(KeyCode.S) && playerMP >= 20)
            StartCoroutine(PlayerChargeAttack());

        if (Input.GetKeyDown(KeyCode.D))
            StartCoroutine(PlayerHeal());
    }

    // ================= PLAYER =================

    IEnumerator PlayerAttack()
    {
        state = TurnState.Busy;
        UpdateTurnUI();

        playerAnim.SetTrigger("AttackKnight");
        yield return new WaitForSeconds(0.5f);

        enemyHP -= 25;
        if (enemyHP < 0) enemyHP = 0;
        UpdateText();

        enemyAnim.SetTrigger("HurtMonster");
        yield return new WaitForSeconds(0.5f);

        if (enemyHP <= 0)
        {
            enemyAnim.SetTrigger("DeathMonster");
            EndBattleAndLoadScene("CreditScene");
            yield break;
        }

        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerChargeAttack()
    {
        state = TurnState.Busy;
        UpdateTurnUI();

        playerMP -= 20;
        if (playerMP < 0) playerMP = 0;
        UpdateText();

        playerAnim.SetTrigger("ChargeAttack");
        yield return new WaitForSeconds(0.7f);

        enemyHP -= 50;
        if (enemyHP < 0) enemyHP = 0;
        UpdateText();

        enemyAnim.SetTrigger("HurtMonster");
        yield return new WaitForSeconds(0.6f);

        if (enemyHP <= 0)
        {
            enemyAnim.SetTrigger("DeathMonster");
            EndBattleAndLoadScene("CreditScene");
            yield break;
        }

        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerHeal()
    {
        state = TurnState.Busy;
        UpdateTurnUI();

        if (healEffect != null)
            healEffect.SetActive(true);

        playerHP += 10;
        if (playerHP > maxPlayerHP)
            playerHP = maxPlayerHP;

        UpdateText();

        yield return new WaitForSeconds(1f);

        if (healEffect != null)
            healEffect.SetActive(false);

        StartCoroutine(EnemyTurn());
    }

    // ================= ENEMY =================

    IEnumerator EnemyTurn()
    {
        state = TurnState.EnemyTurn;
        UpdateTurnUI();

        yield return new WaitForSeconds(0.8f);

        enemyAnim.SetTrigger("AttackMonster");
        yield return new WaitForSeconds(0.5f);

        playerHP -= 20;
        if (playerHP < 0) playerHP = 0;

        playerAnim.SetTrigger("HurtKnight");

        playerMP += 10;
        if (playerMP > maxPlayerMP)
            playerMP = maxPlayerMP;

        UpdateText();

        yield return new WaitForSeconds(0.6f);

        if (playerHP <= 0)
        {
            playerAnim.SetTrigger("DeathKnight");
            EndBattleAndLoadScene("FailedScene");
            yield break;
        }

        state = TurnState.PlayerTurn;
        UpdateTurnUI();
    }

    // ================= END BATTLE =================

    void EndBattleAndLoadScene(string sceneName)
    {
        if (battleEnded) return;

        battleEnded = true;
        StopAllCoroutines();          // 🔒 HENTIKAN SEMUA PROSES
        StartCoroutine(LoadSceneAfterDelay(sceneName));
    }

    IEnumerator LoadSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(sceneName);
    }

    // ================= UI =================

    void UpdateTurnUI()
    {
        yourTurnText.SetActive(state == TurnState.PlayerTurn);
        enemyTurnText.SetActive(state == TurnState.EnemyTurn);
    }

    void UpdateText()
    {
        playerHPText.text = playerHP.ToString();
        playerMPText.text = playerMP.ToString();
        enemyHPText.text = enemyHP.ToString();
    }
}
