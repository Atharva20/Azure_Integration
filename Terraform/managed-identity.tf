# resource "azurerm_role_assignment" "fa-sa-contributor" {
#   count                = var.deploy_tier ? 1 : 0
#   scope                = azurerm_storage_account.clientstorageaccount[0].id
#   role_definition_name = var.storage_access_role_defination
#   principal_id         = azurerm_windows_function_app.azureintegration[0].identity[0].principal_id
# }

# resource "azurerm_role_assignment" "fa-sb-owner" {
#   count                = var.deploy_tier ? 1 : 0
#   scope                = azurerm_servicebus_namespace.servicebus_namespace[0].id
#   role_definition_name = var.servicebus_access_role_defination
#   principal_id         = azurerm_windows_function_app.azureintegration[0].identity[0].principal_id
# }

# resource "azurerm_role_assignment" "la-sa-contributor" {
#   count                = var.deploy_tier ? 1 : 0
#   scope                = azurerm_storage_account.clientstorageaccount[0].id
#   role_definition_name = var.storage_access_role_defination
#   principal_id         = azurerm_logic_app_standard.testlogicapp[0].identity[0].principal_id
# }

# resource "azurerm_role_assignment" "la-sb-owner" {
#   count                = var.deploy_tier ? 1 : 0
#   scope                = azurerm_servicebus_namespace.servicebus_namespace[0].id
#   role_definition_name = var.servicebus_access_role_defination
#   principal_id         = azurerm_logic_app_standard.testlogicapp[0].identity[0].principal_id
# }
