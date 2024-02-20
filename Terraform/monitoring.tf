resource "azurerm_monitor_action_group" "email-alert" {
  count               = var.deploy_tier ? 1 : 0
  name                = "alert-email"
  resource_group_name = azurerm_resource_group.resourcegrp[0].name
  short_name          = "alert-email"

  email_receiver {
    name                    = "azureintegrationteam"
    email_address           = var.alert_email
    use_common_alert_schema = true
  }
}

resource "azurerm_monitor_metric_alert" "blob-threshold" {
  count               = var.deploy_tier ? 1 : 0
  name                = "blob-threshold"
  resource_group_name = azurerm_resource_group.resourcegrp[0].name
  scopes              = [azurerm_storage_account.clientstorageaccount[0].id]
  description         = "Action will be triggered when Transactions count is greater than 50."

  criteria {
    metric_namespace = "Microsoft.Storage/storageAccounts"
    metric_name      = "Transactions"
    aggregation      = "Total"
    operator         = "GreaterThan"
    threshold        = 2
  }

  action {
    action_group_id = azurerm_monitor_action_group.email-alert[0].id
  }

  depends_on = [
    azurerm_monitor_action_group.email-alert,
    azurerm_storage_account.clientstorageaccount
  ]
}
