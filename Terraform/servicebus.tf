# resource "azurerm_servicebus_queue" "sb-qa"{
#   count                    = var.deploy_tier ? 1 : 0
#   name = "asd"
#   namespace_id = ""

#   default_message_ttl = ""
#   dead_lettering_on_message_expiration = true
#   enable_partitioning = false
# }


resource "azurerm_servicebus_namespace" "example" {
  count                    = var.deploy_tier ? 1 : 0
  name                = "servicebus1234512345"
  location            = azurerm_resource_group.rg_dev[0].location
  resource_group_name = azurerm_resource_group.rg_dev[0].name
  sku                 = "Standard"
}

resource "azurerm_servicebus_topic" "example2" {
  count                    = var.deploy_tier ? 1 : 0
  name         = "sbtmaoutbound"
  namespace_id = azurerm_servicebus_namespace.example[0].id

  default_message_ttl = var.servicebus_ttl
  enable_partitioning = true
}



resource "azurerm_servicebus_subscription" "ask049"{
  count                    = var.deploy_tier ? 1 : 0
  name = "ask049-paragon"
  topic_id = azurerm_servicebus_topic.example2[0].id
  max_delivery_count = 10
  default_message_ttl = var.servicebus_ttl
  dead_lettering_on_message_expiration = true
}

resource "azurerm_servicebus_subscription_rule" "example" {
  count                    = var.deploy_tier ? 1 : 0
  name            = "Where_label_is_ask049"
  subscription_id = azurerm_servicebus_subscription.ask049[0].id
  filter_type     = "CorrelationFilter"

  correlation_filter {
    label = "ask049-paragon"
  }
}

resource "azurerm_role_assignment" "example22" {
  scope                = azurerm_servicebus_namespace.example[0].id
  role_definition_name = "Azure Service Bus Data Owner"
  principal_id         = azurerm_windows_function_app.testfunctionapp[0].identity[0].principal_id
}
