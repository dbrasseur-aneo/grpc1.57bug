import grpc
from concurrent import futures

from testsrv_pb2_grpc import TestSrvServicer, add_TestSrvServicer_to_server
from testsrv_pb2 import TestResponse, TestRequest


class Servicer(TestSrvServicer):
    def Test(self, request: TestRequest, context):
        return TestResponse(response_string=f"Received {request.test_string}")


def main():
    # Define agent-worker communication endpoints
    srv_scheme = "unix://"
    srv_endpoint = srv_scheme+"/tmp/srv.sock"

    worker = Servicer()

    # Start worker
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=1))
    add_TestSrvServicer_to_server(worker, server)
    server.add_insecure_port(srv_endpoint)
    server.start()
    print("Worker started")
    server.wait_for_termination()


if __name__ == "__main__":
    main()
