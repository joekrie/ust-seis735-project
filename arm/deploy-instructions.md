# Deployment instructions

https://azure.microsoft.com/en-us/documentation/articles/hdinsight-hadoop-create-windows-clusters-arm-templates/

Using Azure CLI:
azure login
azure config mode arm
azure group create -n hdi1229rg -l "East US 2"
azure group deployment create "hdi1229rg" "hdi1229" --template-file "C:\HDITutorials-ARM\hdinsight-arm-template.json" -p "{\"clusterName\":{\"value\":\"hdi1229win\"},\"clusterStorageAccountName\":{\"value\":\"hdi1229store\"},\"location\":{\"value\":\"East US 2\"},\"clusterLoginPassword\":{\"value\":\"Pass@word1\"}}"