FROM public.ecr.aws/lambda/dotnet:8

WORKDIR /var/task

# Copy everything
COPY ${LAMBDA_TASK_ROOT} .

# Set the CMD to your handler (could also be done as a parameter override outside of the Dockerfile)
CMD ["StudentAPI::StudentAPI.LambdaEntryPoint::FunctionHandlerAsync"]