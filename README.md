# AnimalChess

## 더지니어스에 나오는 십이장기를 모티브로 한 게임
- 12칸이 장기 판.
- 총 2명이서 진행한다.
- 각 유저는 앞으로 만 가는 병사, 대각선으로만 가는 병사, 상하좌우로 가는 병사, 모든 방향으로 이동 가능한 왕 총 4개의 말을 갖고 플레이한다.
- 유저는 3가지 행동중 1가지를 할 수 있다.
- 1 - 병사를 한칸 옮긴다.
- 2 - 해당 위치에 상대방의 병사가 있으면 이를 잡는다.
- 3 - 잡은 병사는 내 진영 위(내 방향 6가지)로 소환할 수 있다.
- 위 행위중 1가지를 행하면 상대방의 차례가 된다.

#### 승리조건
- 상대방의 왕을 잡는다.
- 내 왕이 상대방의 진영에서 1칸이상 버틴다.

### 서버 통신
1. 닉네임 지정
2. 방 확인.
3. 방 입장.
4. 유저 확인.
5. 준비 
6. 시작


### 게임 관리
1. 내 턴인가?
2. 이겼는가?
3. 각 유저 데이터
4. 잡은 말 데이터 관리


### 장기말
1. 이동
2. 이동 가능 위치 보여주기
3. 도착
4. 잡음
5. 소환

### 쫄 : 장기말
1. 이동 - 1칸 이동 (진화한 상태?)
2. 도착 - 끝에 오면 진화
3. 소환

### 왕 : 장기말
1. 이동 - 모든방향
2. 도착 - 끝에 오고 1턴 버티면 승리

### 대각선 : 장기말
1. 이동 - 대각선

### 직선 : 장기말
1. 이동 - 상하좌우

### 테이블
1. 2번 플레이어는 테이블을 회전한다.
2. 처음 말들 셋팅.
3. 각 칸에 데이터 셋팅.
4. 칸에 놓이면 칸에 말 데이터 넣어두기.

### 잡은 말 보여주기
1. 각 플레이어가 잡은 말 보여주기

### 각 칸
1. 말 들을 가지고 있는 칸인가?

### 말 놓기
1. 클릭함을 체크
2. 말을 클릭하면 이동가능 보여주기
3. 말을 클릭한 상태면 땅 이동 가능
4. 땅 누르면 이동가능 땅인가 체크.
5. 땅으로 이동
6. 잡으면 잡은 말 데이터 넣기
7. 잡은말 드래그로 내땅이면 생성

### 게임 통신
1. 말을 이동함
2. 턴이 넘어옴 / 턴을 넘김
3. 승리 / 패배
4. 말을 잡음.