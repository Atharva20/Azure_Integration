variable "location" {
  description = "location for all resources."
  type        = string
}

variable "deploy_tier" {
  description = "wether the resource is required in specific environment or not."
  default     = true
}

variable "instance" {
  description = "The count of the resource."
  type        = string
}

variable "env" {
  description = "The environment to deploy resources in."
  type        = string
}

variable "account_tier" {
  description = "The account tier in which storage account will be billed by."
  type        = string
  default     = "Standard"
}

variable "account_replication_type" {
  description = "In how many regions we want the sa to be replicated"
  type        = string
  default     = "LRS"
}

variable "servicebus_ttl" {
  description = "The life of message in service bus queue or topic if not received by any subscription."
  type        = string
  default     = "P7D"
}

variable "alert_email" {
  description = "The mail-id where the alerts will be delivered."
  type        = string
}

variable "os_type" {
  description = "The operating system, windows/linux etc. on which our asp plan should be deployed."
  type        = string
}

variable "sku_servicebus" {
  description = "The pricing tier in which we want to deploy the servicebus namespace."
  type        = string
}

variable "sku_name_func" {
  description = "The pricing tier in which we want to deploy our function app service plan."
  type        = string
}

variable "sku_name_logic" {
  description = "The pricing tier in which we want to deploy our logic app service plan."
  type        = string
}

variable "storage_access_role_defination" {
  description = "The managed identity role to be assigned to the principal id over the scope(storage account) provided."
  type        = string
}

variable "servicebus_access_role_defination" {
  description = "The managed identity role to be assigned to the principal id over the scope(servicebus) provided."
  type        = string
}

variable "container_access_type" {
  description = "The contianer access such public/private etc. should be specified."
  type        = string
}
