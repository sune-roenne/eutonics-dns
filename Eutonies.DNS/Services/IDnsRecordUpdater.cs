public interface IDnsRecordUpdater {
    Task UpdateIpRecord();

    DateTime? LastUpdate {get;}
}