using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/*public class CustomManager : NetworkManager
{
    public class NetworkMessage : MessageBase
    {
        public int chosenClass ;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        NetworkMessage test = new NetworkMessage();
        test.chosenClass = 0;
        ClientScene.AddPlayer(conn, 0, test);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage>();
        int mes = message.chosenClass;
        GameObject player = Instantiate(spawnPrefabs[mes]) as GameObject;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }




}*/

public class MsgTypes
{
    public const short PlayerPrefab = MsgType.Highest + 1;

    public class PlayerPrefabMsg : MessageBase
    {
        public short controllerID;
        public short prefabIndex;
    }
}

public class CustomManager : NetworkManager
{
    /*  public short playerPrefabIndex=1;

      public override void OnStartServer()
      {
          NetworkServer.RegisterHandler(MsgTypes.PlayerPrefab, OnResponsePrefab);
          base.OnStartServer();
      }

      public override void OnClientConnect(NetworkConnection conn)
      {
          client.RegisterHandler(MsgTypes.PlayerPrefab, OnRequestPrefab);
          base.OnClientConnect(conn);
      }

      private void OnRequestPrefab(NetworkMessage netMsg)
      {
          MsgTypes.PlayerPrefabMsg msg = new MsgTypes.PlayerPrefabMsg();
          msg.controllerID = netMsg.ReadMessage<MsgTypes.PlayerPrefabMsg>().controllerID;
          msg.prefabIndex = playerPrefabIndex;
          client.Send(MsgTypes.PlayerPrefab, msg);
      }

      private void OnResponsePrefab(NetworkMessage netMsg)
      {
          MsgTypes.PlayerPrefabMsg msg = netMsg.ReadMessage<MsgTypes.PlayerPrefabMsg>();
          playerPrefab = spawnPrefabs[msg.prefabIndex];
          base.OnServerAddPlayer(netMsg.conn, msg.controllerID);
          Debug.Log(playerPrefab.name + " spawned!");
      }

      public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
      {
          MsgTypes.PlayerPrefabMsg msg = new MsgTypes.PlayerPrefabMsg();
          msg.controllerID = playerControllerId;
          NetworkServer.SendToClient(conn.connectionId, MsgTypes.PlayerPrefab, msg);
      }*/


    // in the Network Manager component, you must put your player prefabs 
    // in the Spawn Info -> Registered Spawnable Prefabs section 
    public short playerPrefabIndex=0;


    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler(MsgTypes.PlayerPrefab, OnResponsePrefab);
        base.OnStartServer();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        client.RegisterHandler(MsgTypes.PlayerPrefab, OnRequestPrefab);
        base.OnClientConnect(conn);
    }

    private void OnRequestPrefab(NetworkMessage netMsg)
    {
        MsgTypes.PlayerPrefabMsg msg = new MsgTypes.PlayerPrefabMsg();
        msg.controllerID = netMsg.ReadMessage<MsgTypes.PlayerPrefabMsg>().controllerID;
        msg.prefabIndex = playerPrefabIndex;
        client.Send(MsgTypes.PlayerPrefab, msg);
    }

    private void OnResponsePrefab(NetworkMessage netMsg)
    {
        MsgTypes.PlayerPrefabMsg msg = netMsg.ReadMessage<MsgTypes.PlayerPrefabMsg>();
        playerPrefab = spawnPrefabs[msg.prefabIndex];
        base.OnServerAddPlayer(netMsg.conn, msg.controllerID);
        Debug.Log(playerPrefab.name + " spawned!");
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        MsgTypes.PlayerPrefabMsg msg = new MsgTypes.PlayerPrefabMsg();
        msg.controllerID = playerControllerId;
        NetworkServer.SendToClient(conn.connectionId, MsgTypes.PlayerPrefab, msg);
    }
}
