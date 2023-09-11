from testsrv_pb2_grpc import TestSrvStub
from testsrv_pb2 import TestRequest, TestResponse
import grpc

srv_scheme = "unix://"
srv_endpoint = srv_scheme+"/tmp/srv.sock"


def main(name):
    with grpc.insecure_channel(srv_endpoint) as channel:
        stub = TestSrvStub(channel)
        print(" ############ Sending request #############")
        response: TestResponse = stub.Test(TestRequest(test_string=name))
        print(f" ############ {response.response_string} #############")


if __name__ == '__main__':
    main('test')

