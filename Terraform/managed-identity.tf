resource "azurerm_role_assignment" "fa-sa-contributor" {
  count                = var.deploy_tier ? 1 : 0
  scope                = azurerm_storage_account.clientstorageaccount[0].id
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = azurerm_windows_function_app.azureintegration[0].identity[0].principal_id
}

resource "azurerm_role_assignment" "la-sa-contributor" {
  count                = var.deploy_tier ? 1 : 0
  scope                = azurerm_storage_account.clientstorageaccount[0].id
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = azurerm_logic_app_standard.testlogicapp[0].identity[0].principal_id
}

resource "azurerm_role_assignment" "fa-sb-owner" {
  count                = var.deploy_tier ? 1 : 0
  scope                = azurerm_servicebus_namespace.servicebus_namespace[0].id
  role_definition_name = "Azure Service Bus Data Owner"
  principal_id         = azurerm_windows_function_app.azureintegration[0].identity[0].principal_id
}

resource "azurerm_role_assignment" "la-sb-owner" {
  count                = var.deploy_tier ? 1 : 0
  scope                = azurerm_servicebus_namespace.servicebus_namespace[0].id
  role_definition_name = "Azure Service Bus Data Owner"
  principal_id         = azurerm_logic_app_standard.testlogicapp[0].identity[0].principal_id
}
