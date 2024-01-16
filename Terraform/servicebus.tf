resource "azurerm_servicebus_namespace" "servicebus_namespace" {
  count               = var.deploy_tier ? 1 : 0
  name                = local.servicebus_namespace
  location            = azurerm_resource_group.rg_dev[0].location
  resource_group_name = azurerm_resource_group.rg_dev[0].name
  sku                 = "Standard"
}

# resource "azurerm_servicebus_queue" "sb-qa"{
#   count                    = var.deploy_tier ? 1 : 0
#   name = "outbound-queue"
#   namespace_id = azurerm_servicebus_namespace.servicebus_namespace[0].id

#   default_message_ttl = var.servicebus_ttl
#   dead_lettering_on_message_expiration = true
#   enable_partitioning = false
# }

resource "azurerm_servicebus_topic" "sbt-azureint-outbound" {
  count        = var.deploy_tier ? 1 : 0
  name         = "sbt-azureint-outbound"
  namespace_id = azurerm_servicebus_namespace.servicebus_namespace[0].id

  default_message_ttl = var.servicebus_ttl
  enable_partitioning = true
}

resource "azurerm_servicebus_subscription" "ask049" {
  count                                = var.deploy_tier ? 1 : 0
  name                                 = "outbound-routing"
  topic_id                             = azurerm_servicebus_topic.sbt-azureint-outbound[0].id
  max_delivery_count                   = 10
  default_message_ttl                  = var.servicebus_ttl
  dead_lettering_on_message_expiration = true
}

resource "azurerm_servicebus_subscription_rule" "outbound_subs_corelation_filter" {
  count           = var.deploy_tier ? 1 : 0
  name            = "outbound_subs_corelation_filter"
  subscription_id = azurerm_servicebus_subscription.ask049[0].id
  filter_type     = "CorrelationFilter"

  correlation_filter {
    label = "outbound_subs"
  }
}
