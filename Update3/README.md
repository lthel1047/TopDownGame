
Wave 마다 Ai 기능을 다르게 추가할 수 있게 코드 추가

[System.Serializable]
	public class Wave {
		public bool infinite;
		public int enemyCount;			// 생성될 최대 수
		public float timeBetweenSpawns; // 나오는 간격

		// AI Wave 마다 변화를 주기 위한 추가 변수
		public float moveSpeed;			// AI 스피드
		public int hitsToKillPlayer;	// 플레이어가 죽여야 하는 수
		public float enemyHealth;
		public Color skinColur;			// 적 색
	}
  
  
  
  // 스폰 되는 위치 가시성 증가
	IEnumerator SpawnEnemy() {
		float spawnDelay = 1;
		float tileFlashSpeed = 4;

		Transform spawnTile = map.GetRandomOpenTile ();
		if (isCamping) {
			spawnTile = map.GetTileFromPosition(playerT.position);
		}
		Material tileMat = spawnTile.GetComponent<Renderer> ().material;
		Color initialColour = tileMat.color;
		Color flashColour = Color.red;
		float spawnTimer = 0;

		while (spawnTimer < spawnDelay) {

			tileMat.color = Color.Lerp(initialColour,flashColour, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

			spawnTimer += Time.deltaTime;
			yield return null;
		}

		Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
		spawnedEnemy.OnDeath += OnEnemyDeath;
		**spawnedEnemy.SetCharacteristics (currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColur);**
	}
