resource "azurerm_storage_account" "teststorage" {
  count                    = var.deploy_tier ? 1 : 0
  name                     = local.storage_account_name
  resource_group_name      = azurerm_resource_group.rg_dev[0].name
  location                 = azurerm_resource_group.rg_dev[0].location
  account_tier             = var.account_tier
  account_replication_type = var.account_replication_type

  tags = {
    environment = "dev"
  }

  #   identity {
  #     type         = "UserAssigned"
  #     identity_ids = [azurerm_user_assigned_identity.mi[0].id]
  #   }
}

resource "azurerm_storage_container" "client" {
  count                 = var.deploy_tier ? 1 : 0
  name                  = "clientstorageaccount"
  storage_account_name  = azurerm_storage_account.teststorage[0].name
  container_access_type = "private"
}
//outputstorageaccount


resource "azurerm_role_assignment" "funcexample" {
  scope                = azurerm_storage_account.teststorage[0].id
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = azurerm_windows_function_app.testfunctionapp[0].identity[0].principal_id
}

resource "azurerm_role_assignment" "logicexample" {
  scope                = azurerm_storage_account.teststorage[0].id
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = azurerm_logic_app_standard.testlogicapp[0].identity[0].principal_id
}

# data "azurerm_subscription" "primary" {
# }

# resource "azurerm_role_assignment" "rbac" {
#   principal_id         = azurerm_user_assigned_identity.mi[0].principal_id
#   role_definition_name = "Contributor"
#   scope                = azurerm_storage_account.asbstorageacc[0].id
# }

# resource "azurerm_role_assignment" "rbac1" {
#   principal_id         = azurerm_user_assigned_identity.mi[0].principal_id
#   role_definition_name = "Contributor"
#   scope                = data.azurerm_subscription.primary.id
# }

# resource "azurerm_storage_container" "asbcontainer" {
#   count                 = var.is_dev ? 1 : 0
#   name                  = "asbcontainer"
#   storage_account_name  = azurerm_storage_account.asbstorageacc[0].name
#   container_access_type = "private"
# }

# resource "azurerm_storage_container" "logicAppTrigger" {
#   count                 = var.is_dev ? 1 : 0
#   name                  = "logicapptrigger"
#   storage_account_name  = azurerm_storage_account.asbstorageacc[0].name
#   container_access_type = "private"
# }