# TopDownGame

TopDownGame 제작 과정입니다.  

맵 관리에 편의성을 위해 유니티에디터 사용

중복된 맵을 만들지 않기 위해 셔플랜덤 알고리즘 사용
<pre>
<code>
		// 좌표 
		allTileCoords = new List<Coord> ();
		for (int x = 0; x < currentMap.mapSize.x; x ++) {
			for (int y = 0; y < currentMap.mapSize.y; y ++) {
				allTileCoords.Add(new Coord(x,y));
			}
		}
		shuffledTileCoords = new Queue<Coord> (Utility.ShuffleArray (allTileCoords.ToArray (), currentMap.seed));

		// hierarchy 정리용 오브젝트
		string holderName = "Generated Map";        // 타일 오브젝트 자식들 관리
		if (transform.Find (holderName)) {
			DestroyImmediate (transform.Find (holderName).gameObject);  // Immediate를 사용 한 것은 에디터를 사용하기 위함
		}
		
		Transform mapHolder = new GameObject (holderName).transform;    // 새롭게 타일 생성
		mapHolder.parent = transform;

		// 맵 타일 배치
		for (int x = 0; x < currentMap.mapSize.x; x ++) {
			for (int y = 0; y < currentMap.mapSize.y; y ++) {
				Vector3 tilePosition = CoordToPosition(x,y);
				Transform newTile = Instantiate (tilePrefab, tilePosition, Quaternion.Euler (Vector3.right * 90)) as Transform;
				newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
				newTile.parent = mapHolder;
				tileMap[x,y] = newTile;
			}
		}

		// 장애물 생성
		bool[,] obstacleMap = new bool[(int)currentMap.mapSize.x,(int)currentMap.mapSize.y];
		
		int obstacleCount = (int)(currentMap.mapSize.x * currentMap.mapSize.y * currentMap.obstaclePercent);
		int currentObstacleCount = 0;
		List<Coord> allOpenCoords = new List<Coord> (allTileCoords);
		
		for (int i =0; i < obstacleCount; i ++) {
			Coord randomCoord = GetRandomCoord();
			obstacleMap[randomCoord.x,randomCoord.y] = true;
			currentObstacleCount ++;
			
			if (randomCoord != currentMap.mapCentre && MapIsFullyAccessible(obstacleMap, currentObstacleCount)) {
				float obstacleHeight = Mathf.Lerp(currentMap.minObstacleHeight,currentMap.maxObstacleHeight,(float)prng.NextDouble());
				Vector3 obstaclePosition = CoordToPosition(randomCoord.x,randomCoord.y);
				
				Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * obstacleHeight/2, Quaternion.identity) as Transform;
				newObstacle.parent = mapHolder;
				newObstacle.localScale = new Vector3((1 - outlinePercent) * tileSize, obstacleHeight, (1 - outlinePercent) * tileSize);

				Renderer obstacleRenderer = newObstacle.GetComponent<Renderer>();
				Material obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);
				float colourPercent = randomCoord.y / (float)currentMap.mapSize.y;
				obstacleMaterial.color = Color.Lerp(currentMap.foregroundColour,currentMap.backgroundColour,colourPercent);
				obstacleRenderer.sharedMaterial = obstacleMaterial;

				allOpenCoords.Remove(randomCoord);
			}
			else {
				obstacleMap[randomCoord.x,randomCoord.y] = false;
				currentObstacleCount --;
			}
		}
		// 셔플된 좌표를 가진 큐
		shuffledOpenTileCoords = new Queue<Coord> (Utility.ShuffleArray (allOpenCoords.ToArray (), currentMap.seed));

</pre>
</code>  
