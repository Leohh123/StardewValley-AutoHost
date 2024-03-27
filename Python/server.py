from http.server import BaseHTTPRequestHandler, HTTPServer
import time
from controller import Controller


hostName = 'localhost'
serverPort = 9246
controller = Controller()


class MyServer(BaseHTTPRequestHandler):
    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)

    def do_GET(self):
        self.send_response(200)
        self.send_header('Content-type', 'text/html')
        self.end_headers()
        print(controller)
        if controller.is_busy:
            self.wfile.write(bytes('busy', 'utf-8'))
            return
        if self.path == '/record/zoom':
            controller.record_zoom_pos()
            self.wfile.write(bytes('ok', 'utf-8'))
        elif self.path == '/record/sleep':
            controller.record_sleep_pos()
            self.wfile.write(bytes('ok', 'utf-8'))
        elif self.path == '/record/unsleep':
            controller.record_unsleep_pos()
            self.wfile.write(bytes('ok', 'utf-8'))
        elif self.path == '/zoom-and-sleep':
            controller.zoom_and_sleep()
            self.wfile.write(bytes('ok', 'utf-8'))
        elif self.path == '/zoom-and-unsleep':
            controller.zoom_and_unsleep()
            self.wfile.write(bytes('ok', 'utf-8'))
        elif self.path == '/test':
            self.wfile.write(bytes('test', 'utf-8'))
        else:
            self.wfile.write(bytes('invalid', 'utf-8'))


if __name__ == '__main__':
    webServer = HTTPServer((hostName, serverPort), MyServer)
    print('Server started http://%s:%s' % (hostName, serverPort))

    webServer.serve_forever()
