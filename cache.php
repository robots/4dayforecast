<?php
    class Cache {

        private $_data = array();
        private $_filename;
        private $_timeout;

        function __construct ($filename, $timeout = 240) {
            $this->_filename = $filename;
            $this->_timeout = $timeout;
        }

        public function get_key($key) {
            if (file_exists($this->_filename)) {
                $fp = fopen($this->_filename, "r");

                if ($fp) {
                    if (flock($fp, LOCK_SH)) {
                        $contents = stream_get_contents($fp);
                        $this->_data = unserialize($contents);
                        flock($fp, LOCK_UN);
                    }
                    fclose($fp);
                }

                if (is_null($this->_data)) {
                    return null;
                }
            }

            if (array_key_exists($key, $this->_data)) {
                $data = $this->_data[$key];

                if (time() - $data['ts'] > 0) {
                    return null;
                }

                return $data['data'];
            }

            return null;
        }

        public function unset_key($key) {
            $ret = false;

            $fp = fopen($this->_filename, "c+");
            if (flock($fp, LOCK_EX)) {
                fseek($fp, 0);
                $contents = stream_get_contents($fp);
                $this->_data = unserialize($contents);

                if (is_array($this->_data)) {
                    unset($this->_data[$key]);

                    ftruncate($fp, 0);
                    fseek($fp, 0);
                    fwrite($fp, serialize($this->_data));
                    fflush($fp);
                }

                flock($fp, LOCK_UN);

                $ret = true;
            } else {
                $ret = false;
            }

            fclose($fp);
            return $ret;
        }

        public function set_key($key, $data) {
            $ret = false;

            $fp = fopen($this->_filename, "c+");
            if (flock($fp, LOCK_EX)) {
                $contents = stream_get_contents($fp);
                $this->_data = unserialize($contents);

                if (!is_array($this->_data)) {
                    $this->_data = array();
                }

                // timeout \in (1*timeout, 2.5*timeout) hours
                $timeout = $this->_timeout;
                //$timeout *= mt_rand(10, 25) / 10;
                $timeout *= 60;
                $timeout += time();

                $this->_data[$key] = array('ts' => $timeout, 'data' => $data);

                ftruncate($fp, 0);
                fseek($fp, 0);
                fwrite($fp, serialize($this->_data));
                fflush($fp);
                flock($fp, LOCK_UN);

                $ret = true;
            } else {
                $ret = false;
            }

            fclose($fp);
            return $ret;
        }

        public static function clear($key) {
            @unlink($this->_filename);
        }
    }
?>