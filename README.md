# Keio Info - 電車出発案内表示アプリ

## 概要

Unityで制作した、駅のデジタルサイネージ風の電車出発案内表示アプリケーションです。
指定された2方面（新宿駅と京王八王子駅）の直近の出発時刻を最大2件ずつ表示します。
現在時刻に応じて、平日ダイヤと休日ダイヤを自動で切り替えて表示します。

## 主な機能

- **リアルタイム時刻表示**: 現在の日付と時刻をデジタル時計で表示します。
- **出発時刻案内**: 2つの方面別に、次に出発する電車の時刻を2件表示します。
- **ダイヤ自動切替**: 日本の祝日を含む、平日と土休日を自動で判定し、対応するダイヤを表示します。
- **UIテーマ**: 平日は青系、土休日は赤系のテーマカラーにUIが変化します。
- **簡単なデータ管理**: 電車の時刻表はJSONファイルで管理されており、簡単に追加・編集が可能です。

## スクリーンショット

![image](https://user-images.githubusercontent.com/xxxxxxxx/xxxxxxxxxxxxxxxx.png)

## セットアップと実行方法

1.  このリポジトリをクローンまたはダウンロードする。
2.  Unity Hubでプロジェクトフォルダ (`Keio_Info`) を開く。
3.  Unityエディタが起動したら、`Assets/Scenes/TimeInfo.unity` ファイルを開く。
4.  エディタ上部の再生ボタン ▶ をクリックしてアプリケーションを実行する。
5.  APKとしてandroid端末にダウンロードできる。

### 依存関係

- Unity (本プロジェクトは 2022.3.x LTS で開発)
- Universal Render Pipeline (URP)

## プロジェクトの構造

```
...
└───Assets/
    ├───Scenes/
    │   └───TimeInfo.unity          # メインシーン
    ├───Scripts/
    │   ├───TimeManager.cs          # 時刻管理とダイヤ判定のメインロジック
    │   ├───UIController.cs         # UIの表示更新
    │   ├───TrainSchedule.cs        # 時刻表データの構造
    │   └───TrainTime.cs            # 個別の時刻データの構造
    ├───Resources/
    │   └───Jsons/
    │       ├───h_times_weekday.json # 八王子方面・平日ダイヤ
    │       ├───h_times_weekend.json # 八王子方面・土休日ダイヤ
    │       ├───s_times_weekday.json # 新宿方面・平日ダイヤ
    │       └───s_times_weekend.json # 新宿方面・土休日ダイヤ
    ├───Images/                     # UIに使用する画像アセット
    │   └───Clock/                  # デジタル時計用の数字画像
    └───Fonts/                      # フォントファイル
```

## カスタマイズ

### 時刻表の編集

時刻表データは `Assets/Resources/Jsons/` 内のJSONファイルで管理されている。
テキストエディタでこれらのファイルを開き、時刻を編集することで、表示される内容を変更できる。

#### フォーマット

各JSONファイルは、`hour`（時）と `minute`（分）のペアの配列で構成されている。

```json
{
  "trainTimes": [
    {
      "hour": 5,
      "minute": 30
    },
    {
      "hour": 5,
      "minute": 45
    },
    ...
  ]
}
```

## 🛠️ 使用技術

- Unity
- C#
