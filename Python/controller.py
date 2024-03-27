import pyautogui


class Controller:
    def __init__(self):
        self.moves = []
        self.pos_zoom = pyautogui.Point(100, 100)
        self.pos_sleep = pyautogui.Point(200, 200)
        self.pos_unsleep = pyautogui.Point(300, 300)

    @property
    def is_busy(self):
        return len(self.moves) > 0

    class Move:
        def __init__(self, outer, name):
            self.outer = outer
            self.name = name

        def __enter__(self):
            self.outer.moves.append(self.name)

        def __exit__(self, *args):
            self.outer.moves.pop()

    def record_pos(self):
        with self.Move(self, 'record_pos'):
            for i in range(5, 0, -1):
                print(i)
                pyautogui.sleep(1)
            pos = pyautogui.position()
            print(f'recorded: pos = {pos}')
            return pos

    def record_zoom_pos(self):
        with self.Move(self, 'record_zoom_pos'):
            self.pos_zoom = self.record_pos()

    def record_sleep_pos(self):
        with self.Move(self, 'record_sleep_pos'):
            self.pos_sleep = self.record_pos()

    def record_unsleep_pos(self):
        with self.Move(self, 'record_unsleep_pos'):
            self.pos_unsleep = self.record_pos()

    def click(self, pos):
        with self.Move(self, f'click{pos}'):
            pyautogui.moveTo(pos)
            pyautogui.sleep(0.5)
            pyautogui.click()
            pyautogui.sleep(0.5)

    def press(self, pos):
        with self.Move(self, f'press{pos}'):
            pyautogui.moveTo(pos)
            pyautogui.sleep(0.5)
            pyautogui.mouseDown()
            pyautogui.mouseUp()
            pyautogui.sleep(0.5)

    def zoom_and_sleep(self):
        with self.Move(self, 'zoom_and_sleep'):
            self.click(self.pos_zoom)
            self.press(self.pos_sleep)
            self.click(self.pos_zoom)

    def zoom_and_unsleep(self):
        with self.Move(self, 'zoom_and_unsleep'):
            self.click(self.pos_zoom)
            self.press(self.pos_unsleep)
            self.click(self.pos_zoom)
