# resource "azurerm_storage_account" "logicapp_storage_account" {
#   count                    = var.deploy_tier ? 1 : 0
#   name                     = local.logic_storage_account_name
#   resource_group_name      = azurerm_resource_group.rg_dev[0].name
#   location                 = azurerm_resource_group.rg_dev[0].location
#   account_tier             = var.account_tier
#   account_replication_type = var.account_replication_type
# }

resource "azurerm_storage_account" "functionapp_storage_account" {
  count                    = var.deploy_tier ? 1 : 0
  name                     = local.functionapp_storage_account_name
  resource_group_name      = azurerm_resource_group.rg_dev[0].name
  location                 = azurerm_resource_group.rg_dev[0].location
  account_tier             = var.account_tier
  account_replication_type = var.account_replication_type
}

resource "azurerm_storage_account" "clientstorageaccount" {
  count                    = var.deploy_tier ? 1 : 0
  name                     = local.storage_account_name
  resource_group_name      = azurerm_resource_group.rg_dev[0].name
  location                 = azurerm_resource_group.rg_dev[0].location
  account_tier             = var.account_tier
  account_replication_type = var.account_replication_type

  #   identity {
  #     type         = "UserAssigned"
  #     identity_ids = [azurerm_user_assigned_identity.mi[0].id]
  #   }
}

resource "azurerm_storage_container" "client" {
  count                 = var.deploy_tier ? 1 : 0
  name                  = "inboundshipmetdata"
  storage_account_name  = azurerm_storage_account.clientstorageaccount[0].name
  container_access_type = "private"
}

resource "azurerm_storage_container" "client" {
  count                 = var.deploy_tier ? 1 : 0
  name                  = "outboundshipmetdata"
  storage_account_name  = azurerm_storage_account.clientstorageaccount[0].name
  container_access_type = "private"
}
