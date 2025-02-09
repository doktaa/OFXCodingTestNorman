## Submission for the OFX Software Engineer coding test; to create a Currency API.

### ASSUMPTIONS
- When creating a transfer, account number should have validation checking for numeric characters only, and return appropriate validation prompts
- When creating a transfer, assume that Payer and Recipient information does not need to reflect existing entities, and that provided ID's and information is assumed to be correct if valid
- Exchange rate caching : Only rates based on sell-buy currencies will be cached, and will exclude context of rate datetime as well as converted amount. If we are caching the spotrate datetimes of rates as well as amounts converted, there is a very low chance that cache data will be re-accessed, thus defeating its purpose.

### IMPROVEMENTS
- At the moment, validation will return information based on the first invalid occurrence. This can be improved so that information on all invalid occurrences can be returned to the consumer for better clarity/visibility. For example, if a create quote payload contains both incorrect currencies and amount information, currency validation will be returned since it checks first. Ideally you'd return currency and amount validation messages together.
- Validation currently exists in their respective services. There is opportunity to refactor such that validation can happen as an extension method of DTO classes or individual IValidator implementations.
- Global exception handling
- Minor: Validation error messages can be moved to their own constant classes, so that it can be referencced in both code as well as tests. This will also enable more specific testing for exception assertions in tests.
