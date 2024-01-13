resource "azurerm_role_assignment" "funcexample" {
  scope                = azurerm_storage_account.teststorage[0].id
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = azurerm_windows_function_app.testfunctionapp[0].identity[0].principal_id
}

resource "azurerm_role_assignment" "example22" {
  scope                = azurerm_servicebus_namespace.example[0].id
  role_definition_name = "Azure Service Bus Data Owner"
  principal_id         = azurerm_windows_function_app.testfunctionapp[0].identity[0].principal_id
}
